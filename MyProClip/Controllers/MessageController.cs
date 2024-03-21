using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyProClip.Hubs;
using MyProClip.Models;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using MyProClip_BLL.Services;
using System.Security.Claims;

namespace MyProClip.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IFriendshipService _friendshipService;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;

        public MessageController(IHubContext<ChatHub> hubContext, IFriendshipService friendshipService, IUserService userService, UserManager<IdentityUser> userManager) 
        {
            _hubContext = hubContext;
            _friendshipService = friendshipService;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageViewModel sendMessageViewModel)
        {
            string userId = GetUserIdFromClaims();

            IdentityUser? friendUser = await _userService.FindUserByNameAsync(sendMessageViewModel.ReceiverName);
            if (friendUser == null)
            {
                return NotFound("Friend not found.");
            }

            IdentityUser? sender = await _userManager.FindByIdAsync(userId);
            if (sender == null)
            {
                return NotFound("User not found.");
            }

            bool friendshipExists = await _friendshipService.FriendshipExists(friendUser.Id, sender.Id);
            if (!friendshipExists)
            {
                return NotFound("Friendship doesn't exist.");
            }

            await _hubContext.Clients.All.SendAsync("SendMessage", friendUser.UserName, sendMessageViewModel.message);

            return Ok();
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
