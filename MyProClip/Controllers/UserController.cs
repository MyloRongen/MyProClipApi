using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProClip.Models;
using MyProClip_BLL.Exceptions.User;
using System.Security.Claims;

namespace MyProClip.Controllers
{
    [Route("api/")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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

                IdentityUser? user = await _userManager.FindByIdAsync(userId);
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

                IdentityUser? user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(new { username = user.UserName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to get username: {ex.Message}");
            }
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
