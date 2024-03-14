using MyProClip_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Interfaces.Services
{
    public interface IMessageService
    {
        Task<Message> CreateMessageAsync(string senderId, string receiverId, string messageString, int clipId);
        Task<List<Message>> GetMessagesAsync(string userId, string friendId);
    }
}
