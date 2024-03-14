using MyProClip_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Interfaces.Repositories
{
    public interface IFriendshipRepository
    {
        Task CreateFriendship(FriendShip friendship);
        Task<bool> FriendshipExists(string userId, string friendId);
        Task<List<FriendShip>> GetPendingFriendRequests(string userId);
        Task AcceptFriendRequest(FriendShip friendship);
        Task<FriendShip?> GetFriendshipByUserId(int friendshipId);
        Task DeleteFriendship(FriendShip friendship);
        Task<List<FriendShip>> GetFriendsById(string userId);
    }
}
