using Microsoft.EntityFrameworkCore;
using MyProClip_BLL.Enums;
using MyProClip_BLL.Exceptions.Friendship;
using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_BLL.Models;
using MyProClip_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_DAL.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FriendshipRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateFriendship(FriendShip friendship)
        {
            try
            {
                _dbContext.Friendships.Add(friendship);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new FriendshipCreationException("Error creating friendship.", ex);
            }
        }

        public async Task<bool> FriendshipExists(string userId, string friendId)
        {
            try
            {
                bool userToFriendExists = await _dbContext.Friendships.AnyAsync(f => f.UserId == userId && f.FriendId == friendId);
                bool friendToUserExists = await _dbContext.Friendships.AnyAsync(f => f.UserId == friendId && f.FriendId == userId);

                return userToFriendExists || friendToUserExists;
            }
            catch (Exception ex)
            {
                throw new FriendshipCheckException("Error checking friendship.", ex);
            }
        }

        public async Task<List<FriendShip>> GetPendingFriendRequests(string userId)
        {
            try
            {
                return await _dbContext.Friendships
                    .Where(f => f.FriendId == userId && f.Status == FriendshipStatus.Pending)
                    .Include(f => f.User)
                    .Include(f => f.Friend)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new PendingFriendRequestsRetrievalException("Error retrieving pending friend requests.", ex);
            }
        }

        public async Task AcceptFriendRequest(FriendShip friendship)
        {
            try
            {
                _dbContext.Friendships.Update(friendship);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new FriendRequestAcceptanceException("Error accepting friend request.", ex);
            }
        }

        public async Task DeleteFriendship(FriendShip friendship)
        {
            try
            {
                _dbContext.Friendships.Remove(friendship);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new FriendshipDeletionException("Error deleting friendship.", ex);
            }
        }

        public async Task<FriendShip?> GetFriendshipByUserId(int friendshipId)
        {
            try
            {
                return await _dbContext.Friendships.FindAsync(friendshipId);
            }
            catch (Exception ex)
            {
                throw new FriendshipRetrievalException("Error retrieving friendship by ID.", ex);
            }
        }

        public async Task<List<FriendShip>> GetFriendsById(string userId)
        {
            try
            {
                return await _dbContext.Friendships
                    .Where(f => f.FriendId == userId || f.UserId == userId)
                    .Where(f => f.Status == FriendshipStatus.Accepted)
                    .Include(f => f.User)
                    .Include(f => f.Friend)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new FriendshipRetrievalException("Error retrieving friends.", ex);
            }
        }
    }
}
