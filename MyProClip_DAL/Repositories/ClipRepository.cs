using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyProClip_BLL.Enums;
using MyProClip_BLL.Exceptions;
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
    public class ClipRepository : IClipRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ClipRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Clip>> GetClipsByUserId(string userId)
        {
            try
            {
                List<Clip> clips = await _dbContext.Clips
                    .Include(c => c.User)
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                return clips;
            }
            catch (Exception ex)
            {
                throw new ClipRetrievalException("Error retrieving clips by user ID.", ex);
            }
        }

        public async Task<List<Clip>> GetPublicClips()
        {
            try
            {
                List<Clip> clips = await _dbContext.Clips
                    .Include(c => c.User)
                    .Where(c => c.Privacy == PrivacyType.Public)
                    .ToListAsync();

                return clips;
            }
            catch (Exception ex)
            {
                throw new ClipRetrievalException("Error retrieving public clips.", ex);
            }
        }

        public void AddClip(Clip clip)
        {
            try
            {
                _dbContext.Clips.Add(clip);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ClipAdditionException("Error adding a clip.", ex);
            }
        }

        public async Task<Clip?> GetClipById(int clipId)
        {
            try
            {
                return await _dbContext.Clips.FindAsync(clipId);
            }
            catch (Exception ex)
            {
                throw new ClipRetrievalException("Error retrieving clip by ID.", ex);
            }
        }

        public async Task DeleteClipAsync(Clip clip)
        {
            try
            {
                _dbContext.Clips.Remove(clip);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ClipDeletionException("Error deleting a clip.", ex);
            }
        }
    }
}
