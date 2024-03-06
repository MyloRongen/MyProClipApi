﻿using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Message> CreateMessageAsync(string senderId, string receiverId, string messageString)
        {
            if (string.IsNullOrEmpty(senderId) || string.IsNullOrEmpty(receiverId) || string.IsNullOrEmpty(messageString))
            {
                throw new ArgumentException("Sender, receiver or the message is unkown.");
            }

            Message message = new()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = messageString,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            return await _messageRepository.CreateMessageAsync(message);
        }

        public async Task<List<Message>> GetMessagesAsync(string userId, string friendId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(friendId))
            {
                throw new ArgumentException("user or friend cannot be found!");
            }

            return await _messageRepository.GetMessagesAsync(userId, friendId);
        }
    }
}
