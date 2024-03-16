using Microsoft.EntityFrameworkCore;
using MyProClip_BLL.Exceptions.Message;
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
            catch (Exception ex)
            {
                throw new MessageCreationException("Error creating message.", ex);
            }
        }

        public async Task<List<Message>> GetMessagesAsync(string userId, string friendId)
        {
            try
            {
                return await _dbContext.Messages
                    .Where(m => (m.SenderId == userId && m.ReceiverId == friendId) || (m.SenderId == friendId && m.ReceiverId == userId))
                    .Include(m => m.Clip)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new MessageRetrievalException("Error retrieving messages.", ex);
            }
        }
    }
}
