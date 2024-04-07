using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProClip.Models;
using MyProClip_BLL.Exceptions.Clip;
using MyProClip_BLL.Exceptions.User;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using MyProClip_BLL.Services;

namespace MyProClip.Controllers
{
    [Route("api/")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ReportedClipController : ControllerBase
    {
        private readonly IReportedClipService _reportedClipService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClipService _clipService;

        public ReportedClipController(IReportedClipService reportedClipService, UserManager<ApplicationUser> userManager,  IClipService clipService)
        {
            _reportedClipService = reportedClipService;
            _userManager = userManager;
            _clipService = clipService;
        }

        [HttpGet("reportedClips")]
        public async Task<IActionResult> GetReportedClips()
        {
            try
            {
                List<ReportUserClip> reportUserClips = await _reportedClipService.GetReportedClips();

                List<ReportedClipViewModel> reportedClipViewModels = [];

                foreach (ReportUserClip reportUserClip in reportUserClips)
                {
                    ReportedClipViewModel reportClipViewModel = new()
                    {
                        Id = reportUserClip.Id,
                        UserId = reportUserClip.UserId,
                        ReporterId = reportUserClip.ReporterId,
                        ReporterName = reportUserClip.Reporter.UserName,
                        Username = reportUserClip.User.UserName,
                        Clip = new ClipViewModel()
                        {
                            Id = reportUserClip.Clip.Id,
                            Title = reportUserClip.Clip.Title,
                            UserName = reportUserClip.User.Id,
                            ThumbnailSrc = reportUserClip.Clip.ThumbnailUrl,
                            VideoSrc = reportUserClip.Clip.VideoUrl,
                            Privacy = reportUserClip.Clip.Privacy,
                            UpdatedAt = reportUserClip.Clip.UpdatedAt,
                            CreatedAt = reportUserClip.Clip.CreatedAt,
                        },
                        Reason = reportUserClip.Reason,
                    };

                    reportedClipViewModels.Add(reportClipViewModel);
                }

                return Ok(reportedClipViewModels);
            }
            catch (Exception ex)
            {
                return NotFound($"Failed to get reported clips: {ex.Message}");
            }
        }

        [HttpPost("acceptReport/{reportedClipId}")]
        public async Task<IActionResult> AcceptReport(int reportedClipId)
        {
            try
            {
                ReportUserClip? reportedClip = await _reportedClipService.GetReportUserClipById(reportedClipId);
                if (reportedClip == null)
                {
                    return NotFound("Reported clip was not found!");
                }

                ApplicationUser? user = await _userManager.FindByIdAsync(reportedClip.UserId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.IsBanned = true;
                user.LockoutEnd = DateTimeOffset.MaxValue;

                var resultUpdateUser = await _userManager.UpdateAsync(user);

                if (!resultUpdateUser.Succeeded)
                {
                    return NotFound("Failed to ban user!");
                }

                ApplicationUser? reporter = await _userManager.FindByIdAsync(reportedClip.UserId);
                if (reporter == null)
                {
                    return NotFound("Reporter not found.");
                }

                reporter.Points = reporter.Points += 10;

                var resultUpdateReporter = await _userManager.UpdateAsync(user);

                if (!resultUpdateReporter.Succeeded)
                {
                    return NotFound("Failed to reward the reporter!");
                }

                await _reportedClipService.DeleteReportedClip(reportedClip);
                await _clipService.DeleteClipAsync(reportedClip.Clip);

                return Ok("Report accepted successfully");
            }
            catch (ArgumentNullException ex)
            {
                return NotFound($"Failed to delete a reported clip: {ex.Message}");
            }
            catch (UserReportClipException ex)
            {
                return NotFound($"Failed to delete a reported clip: {ex.Message}");
            }
            catch (UserManagerException ex)
            {
                return NotFound($"Failed to update username: {ex.Message}");
            }
            catch (ClipManagerException ex)
            {
                return NotFound($"Failed to retrieve user clips: {ex.Message}");
            }
            catch (Exception ex)
            {
                return NotFound($"Failed to accept report: {ex.Message}");
            }
        }

        [HttpDelete("reportedClips/{reportedClipId}")]
        public async Task<IActionResult> DeleteReportedClip(int reportedClipId)
        {
            try
            {
                ReportUserClip? reportUserClip = await _reportedClipService.GetReportUserClipById(reportedClipId);
                if (reportUserClip == null)
                {
                    return NotFound("Reported clip was not found!");
                }

                await _reportedClipService.DeleteReportedClip(reportUserClip);

                return Ok("Reported was declined!");
            }
            catch (ArgumentNullException ex)
            {
                return NotFound($"Failed to delete a reported clip: {ex.Message}");
            }
            catch (UserReportClipException ex)
            {
                return NotFound($"Failed to delete a reported clip: {ex.Message}");
            }
        }
    }
}
