using Microsoft.EntityFrameworkCore;
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
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            try
            {
                await _dbContext.Messages.AddAsync(message);
                await _dbContext.SaveChangesAsync();
                return message;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong while storing the message!"); ;
            }
        }

        public async Task<List<Message>> GetMessagesAsync(string userId, string friendId)
        {
            try
            {
                return await _dbContext.Messages
                    .Where(m => (m.SenderId == userId && m.ReceiverId == friendId) || (m.SenderId == friendId && m.ReceiverId == userId))
                    .ToListAsync();
            }
            catch
            {
                throw new Exception("Something went wrong while trying to retrieve the clips.");
            }
        }
    }
}
