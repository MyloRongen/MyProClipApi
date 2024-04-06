using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProClip.Models;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;

namespace MyProClip.Controllers
{
    [Route("api/")]
    [Authorize]
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
                        UserId = reportUserClip.UserId,
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
    }
}
