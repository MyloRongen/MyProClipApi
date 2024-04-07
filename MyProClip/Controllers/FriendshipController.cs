using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProClip.Models;
using MyProClip_BLL.Enums;
using MyProClip_BLL.Exceptions.Friendship;
using MyProClip_BLL.Exceptions.User;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using MyProClip_BLL.Services;
using System.Security.Claims;

namespace MyProClip.Controllers
{
    [Route("api/")]
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

        [HttpGet("friends")]
        public async Task<IActionResult> GetFriendships()
        {
            try
            {
                string userId = GetUserIdFromClaims();
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return NotFound("User cannot be found!");
                }

                List<FriendShip> friendships = await _friendshipService.GetFriendsById(GetUserIdFromClaims());

                if (friendships == null)
                {
                    return Ok(new { message = "You have no friends, you can make them by adding someone." });
                }

                List<FriendsViewModel> friendViewModels = [];

                foreach (FriendShip friendship in friendships)
                {
                    string? friendName = friendship.UserId == userId ? friendship.Friend.UserName : friendship.User.UserName;

                    FriendsViewModel newFriend = new()
                    {
                        FriendId = friendship.Id,
                        FriendName = friendName
                    };

                    friendViewModels.Add(newFriend);
                }

                return Ok(friendViewModels);
            }
            catch (UserManagerException ex)
            {
                return NotFound($"Failed to get friendships: {ex.Message}");
            }
            catch (FriendshipManagerException ex)
            {
                return NotFound($"Failed to get friendships: {ex.Message}");
            }
        }

        [HttpPost("friends")]
        public async Task<IActionResult> CreateFriendship([FromBody] FriendshipViewModel friendshipViewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(friendshipViewModel.FriendName))
                {
                    return NotFound("The name from the friend is empty.");
                }

                ApplicationUser? friendUser = await _userService.FindUserByNameAsync(friendshipViewModel.FriendName);
                if (friendUser == null)
                {
                    return NotFound("Friend not found.");
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
            catch (UserManagerException ex)
            {
                return NotFound($"Failed to create friendship: {ex.Message}");
            }
            catch (FriendshipManagerException ex)
            {
                return NotFound($"Failed to create friendship: {ex.Message}");
            }
        }

        [HttpGet("friends/pending")]
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
            catch (UserManagerException ex)
            {
                return NotFound($"Failed to get pending friend requests: {ex.Message}");
            }
            catch (FriendshipManagerException ex)
            {
                return NotFound($"Failed to get pending friend requests: {ex.Message}");
            }
        }

        [HttpPut("friends/accept")]
        public async Task<IActionResult> AcceptFriendshipRequest([FromBody] AcceptFriendRequestViewModel acceptFriendRequestViewModel)
        {
            try
            {
                ApplicationUser? friendUser = await _userService.FindUserByNameAsync(acceptFriendRequestViewModel.FriendName);
                if (friendUser == null)
                {
                    return NotFound("Friend not found.");
                }

                bool friendshipExists = await _friendshipService.FriendshipExists(friendUser.Id, GetUserIdFromClaims());
                if (!friendshipExists)
                {
                    return NotFound("Friend request doesn't exist.");
                }

                FriendShip? friendship = await _friendshipService.GetFriendshipByUserId(acceptFriendRequestViewModel.FriendshipId);
                if (friendship == null)
                {
                    return NotFound("Friend request doesn't exist.");
                }

                await _friendshipService.AcceptFriendRequest(friendship);

                return Ok("Friend request have been accepted");
            }
            catch (UserManagerException ex)
            {
                return NotFound($"Failed to accept friend request: {ex.Message}");
            }
            catch (FriendshipManagerException ex)
            {
                return NotFound($"Failed to accept friend request: {ex.Message}");
            }
        }

        [HttpDelete("friends")]
        public async Task<IActionResult> DeclineFriendRequest([FromBody] DeleteFriendshipViewModel deleteFriendshipViewModel)
        {
            try
            {
                ApplicationUser? friendUser = await _userService.FindUserByNameAsync(deleteFriendshipViewModel.FriendName);
                if (friendUser == null)
                {
                    return NotFound("Friend not found.");
                }

                bool friendshipExists = await _friendshipService.FriendshipExists(friendUser.Id, GetUserIdFromClaims());
                if (!friendshipExists)
                {
                    return NotFound("Friend request doesn't exist.");
                }

                FriendShip? friendship = await _friendshipService.GetFriendshipByUserId(deleteFriendshipViewModel.FriendshipId);
                if (friendship == null)
                {
                    return NotFound("Friend request doesn't exist.");
                }

                await _friendshipService.DeleteFriendship(friendship);

                return Ok("Friend request has been declined!");
            }
            catch (UserManagerException ex)
            {
                return NotFound($"Failed to decline friend request: {ex.Message}");
            }
            catch (FriendshipManagerException ex)
            {
                return NotFound($"Failed to decline friend request: {ex.Message}");
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
