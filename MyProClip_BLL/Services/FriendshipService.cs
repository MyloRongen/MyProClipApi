﻿using Microsoft.EntityFrameworkCore;
using MyProClip_BLL.Enums;
using MyProClip_BLL.Exceptions.Friendship;
using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;

        public FriendshipService(IFriendshipRepository friendshipRepository)
        {
            _friendshipRepository = friendshipRepository;
        }

        public async Task CreateFriendship(FriendShip friendship)
        {
            if (string.IsNullOrWhiteSpace(friendship.UserId) || string.IsNullOrWhiteSpace(friendship.FriendId))
            {
                throw new InvalidFriendshipDataException("Invalid friend or user.");
            }

            await _friendshipRepository.CreateFriendship(friendship);
        }

        public async Task<bool> FriendshipExists(string userId, string friendId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new InvalidUserIdException("Invalid user id.");
            }

            if (string.IsNullOrWhiteSpace(friendId))
            {
                throw new InvalidFriendIdException("Invalid friend id.");
            }

            return await _friendshipRepository.FriendshipExists(userId, friendId);
        }

        public async Task<List<FriendShip>> GetPendingFriendRequests(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new InvalidUserIdException("Invalid user id.");
            }

            return await _friendshipRepository.GetPendingFriendRequests(userId);
        }

        public async Task AcceptFriendRequest(FriendShip friendship)
        {
            if (string.IsNullOrWhiteSpace(friendship.UserId) || string.IsNullOrWhiteSpace(friendship.FriendId))
            {
                throw new InvalidFriendshipDataException("Invalid friend or user id.");
            }

            friendship.Status = FriendshipStatus.Accepted;

            await _friendshipRepository.AcceptFriendRequest(friendship);
        }

        public async Task<FriendShip?> GetFriendshipByUserId(int friendshipId)
        {
            if (friendshipId <= 0)
            {
                throw new FriendshipNotFoundException("Invalid friendship id.");
            }

            return await _friendshipRepository.GetFriendshipByUserId(friendshipId);
        }

        public async Task DeleteFriendship(FriendShip friendship)
        {
            if (string.IsNullOrWhiteSpace(friendship.UserId) || string.IsNullOrWhiteSpace(friendship.FriendId))
            {
                throw new InvalidFriendshipDataException("Invalid friend or user id.");
            }

            await _friendshipRepository.DeleteFriendship(friendship);
        }

        public async Task<List<FriendShip>> GetFriendsById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new InvalidUserIdException("Invalid user id.");
            }

            return await _friendshipRepository.GetFriendsById(userId);
        }
    }
}
