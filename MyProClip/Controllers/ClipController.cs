using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyProClip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClipController : ControllerBase
    {
        [HttpGet("get-projects")]
        [Authorize(Policy = "Bearer")]
        public async Task<IActionResult> GetClips()
        {
            try
            {
                int userId = GetUserIdFromClaims();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve Clips: {ex.Message}");
            }
        }

        private int GetUserIdFromClaims()
        {
            string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                throw new ArgumentException("User ID not found or invalid.");
            }

            return userId;
        }
    }
}
