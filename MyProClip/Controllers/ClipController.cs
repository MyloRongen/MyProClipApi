using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProClip.Models;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using System.Security.Claims;

namespace MyProClip.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ClipController : ControllerBase
    {
        private readonly IClipService _clipService;

        public ClipController(IClipService clipService)
        {
            _clipService = clipService;
        }

        [HttpGet("get-clips")]
        public async Task<IActionResult> GetClips()
        {
            try
            {
                string userId = GetUserIdFromClaims();

                List<Clip> clips = await _clipService.GetClipsByUserId(userId);

                List<ClipViewModel> clipViewModels = [];

                foreach (Clip clip in clips)
                {
                    ClipViewModel newClip = new()
                    {
                        Id = clip.Id,
                        Title = clip.Title,
                        Content = clip.Content,
                        UserName = clip.User.UserName,
                        Privacy = clip.Privacy,
                        UpdatedAt = clip.UpdatedAt,
                        CreatedAt = clip.CreatedAt
                    };

                    clipViewModels.Add(newClip);
                }

                return Ok(clipViewModels);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve Clips: {ex.Message}");
            }
        }

        private string GetUserIdFromClaims()
        {
            string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                throw new Exception("User ID not found or invalid.");
            }

            return userIdString;
        }
    }
}
