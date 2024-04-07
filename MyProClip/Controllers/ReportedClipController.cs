using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public ReportedClipController(IReportedClipService reportedClipService)
        {
            _reportedClipService = reportedClipService;
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
