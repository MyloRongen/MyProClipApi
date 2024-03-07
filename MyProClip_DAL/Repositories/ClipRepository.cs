using Microsoft.AspNetCore.Identity;
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
            catch
            {
                throw new Exception("Something went wrong while trying to retrieve the clips.");
            }
        }

        public void AddClip(Clip clip)
        {
            try
            {
                _dbContext.Clips.Add(clip);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw new Exception("Something went wrong while trying to add a clip.");
            }
        }

        public async Task<Clip?> GetClipById(int clipId)
        {
            try
            {
                return await _dbContext.Clips.FindAsync(clipId);
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong while trying to get the clip.");
            }
        }
    }
}
