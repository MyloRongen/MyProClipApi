using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProClip.Models;
using MyProClip_BLL.Enums;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using MyProClip_BLL.Services;
using System.Security.Claims;

namespace MyProClip.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;
        private readonly IUserService _userService;

        public FriendshipController(IFriendshipService friendshipService, IUserService userService)
        {
            _friendshipService = friendshipService;
            _userService = userService;
        }

        [HttpPost("add-friend")]
        public async Task<IActionResult> CreateFriendship([FromBody] FriendshipViewModel friendshipViewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(friendshipViewModel.FriendName))
                {
                    return BadRequest("The name from the friend is empty.");
                }

                IdentityUser? friendUser = await _userService.FindUserByNameAsync(friendshipViewModel.FriendName);
                if (friendUser == null)
                {
                    return BadRequest("Friend not found.");
                }

                bool friendshipExists = await _friendshipService.FriendshipExists(GetUserIdFromClaims(), friendUser.Id);
                if (friendshipExists)
                {
                    return Ok(new { message = "Friend request already been sent or accepted." });
                }

                FriendShip friendship = new()
                {
                    UserId = GetUserIdFromClaims(),
                    FriendId = friendUser.Id,
                    Status = FriendshipStatus.Pending
                };

                await _friendshipService.CreateFriendship(friendship);

                return Ok(new { message = "Friend request sent!" });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("pending-friend-requests")]
        public async Task<IActionResult> GetPendingFriendRequests()
        {
            try
            {
                string userId = GetUserIdFromClaims();
                List<FriendShip> pendingRequests = await _friendshipService.GetPendingFriendRequests(userId);

                List<FriendRequestViewModel> friendRequestViewModels = [];

                foreach (FriendShip pendingRequest in pendingRequests)
                {
                    FriendRequestViewModel newFriendRequest = new()
                    {
                        FriendRequestId = pendingRequest.Id,
                        FriendName = pendingRequest.User.UserName,
                    };

                    friendRequestViewModels.Add(newFriendRequest);
                }

                return Ok(friendRequestViewModels);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("accept-friend-request")]
        public async Task<IActionResult> AcceptFriendshipRequest([FromBody] AcceptFriendRequestViewModel acceptFriendRequestViewModel)
        {
            try
            {
                IdentityUser? friendUser = await _userService.FindUserByNameAsync(acceptFriendRequestViewModel.FriendName);
                if (friendUser == null)
                {
                    return BadRequest("Friend not found.");
                }

                bool friendshipExists = await _friendshipService.FriendshipExists(friendUser.Id, GetUserIdFromClaims());
                if (!friendshipExists)
                {
                    return BadRequest("Friend request doesn't exist.");
                }

                FriendShip? friendship = await _friendshipService.GetFriendshipByUserId(acceptFriendRequestViewModel.FriendshipId);
                if (friendship == null)
                {
                    return BadRequest("Friend request doesn't exist.");
                }

                await _friendshipService.AcceptFriendRequest(friendship);

                return Ok("Friend request have been accepted");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("decline-friend-request")]
        public async Task<IActionResult> DeclineFriendRequest([FromBody] DeleteFriendshipViewModel deleteFriendshipViewModel)
        {
            try
            {
                IdentityUser? friendUser = await _userService.FindUserByNameAsync(deleteFriendshipViewModel.FriendName);
                if (friendUser == null)
                {
                    return BadRequest("Friend not found.");
                }

                bool friendshipExists = await _friendshipService.FriendshipExists(friendUser.Id, GetUserIdFromClaims());
                if (!friendshipExists)
                {
                    return BadRequest("Friend request doesn't exist.");
                }

                FriendShip? friendship = await _friendshipService.GetFriendshipByUserId(deleteFriendshipViewModel.FriendshipId);
                if (friendship == null)
                {
                    return BadRequest("Friend request doesn't exist.");
                }

                await _friendshipService.DeleteFriendship(friendship);

                return Ok("Friend request has been declined!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
