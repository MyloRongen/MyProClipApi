using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProClip.Models;
using MyProClip_BLL.Exceptions.User;
using System.Security.Claims;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using MyProClip_BLL.Services;
using MyProClip_BLL.Exceptions.Clip;

namespace MyProClip.Controllers
{
    [Route("api/")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IClipService _clipService;

        public UserController(UserManager<ApplicationUser> userManager, IUserService userService, IClipService clipService)
        {
            _userManager = userManager;
            _userService = userService;
            _clipService = clipService;
        }

        [HttpPost("users/updateUsername")]
        public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameViewModel updateUsernameViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return NotFound($"Failed to update username, username was empty!");
                }

                string userId = GetUserIdFromClaims();

                ApplicationUser? user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.UserName = updateUsernameViewModel.NewUsername;
                user.NormalizedUserName = _userManager.NormalizeName(updateUsernameViewModel.NewUsername);

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok("Updated username successfully");
                }
                else
                {
                    throw new UsernameUpdateException($"Failed to update username");
                }
            }
            catch (UserManagerException ex)
            {
                return NotFound($"Failed to update username: {ex.Message}");
            }
        }

        [HttpGet("users/getUsername")]
        public async Task<IActionResult> GetUsername()
        {
            try
            {
                string userId = GetUserIdFromClaims();

                ApplicationUser? user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(new { username = user.UserName, points = user.Points });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to get username: {ex.Message}");
            }
        }

        [HttpPost("users/reportUser")]
        public async Task<IActionResult> ReportUser([FromBody] ReportUserClipViewModel reportModel)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByIdAsync(reportModel.UserId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                ApplicationUser? reporter = await _userManager.FindByIdAsync(reportModel.ReporterId);
                if (reporter == null)
                {
                    return NotFound("Reporter not found.");
                }


                Clip? clip = await _clipService.GetClipById(reportModel.ClipId);

                if (clip == null)
                {
                    return NotFound("The Clip was not found!");
                }


                ReportUserClip reportUserClip = new()
                {
                    UserId = reportModel.UserId,
                    ReporterId = reportModel.ReporterId,
                    ClipId = reportModel.ClipId,
                    Reason = reportModel.Reason
                };

                await _userService.UserReportClip(reportUserClip);

                return Ok("User reported successfully.");
            }
            catch (UserManagerException ex)
            {
                return NotFound($"Failed to report user clip: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return NotFound($"Failed to report user clip: {ex.Message}");
            }
            catch (ClipManagerException ex)
            {
                return NotFound($"Failed to report user clip: {ex.Message}");
            }
        }

        [HttpGet("info")]
        [Authorize]
        public List<string>? GetInfo()
        {
            string? id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
            {
                return null;
            }

            return User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();
        }

        private string GetUserIdFromClaims()
        {
            string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UserRetrievalException("User ID not found or invalid.");
            }

            return userIdString;
        }
    }
}
