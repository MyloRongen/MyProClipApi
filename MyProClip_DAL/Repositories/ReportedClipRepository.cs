using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_BLL.Models;
using MyProClip_DAL.Data;

namespace MyProClip_DAL.Repositories
{
    public class ReportedClipRepository : IReportedClipRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReportedClipRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ReportUserClip>> GetReportedClips()
        {
            try
            {
                return await _dbContext.ReportUserClips
                    .Include(ruc => ruc.User)
                    .Include(ruc => ruc.Clip)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting reported clips.", ex);
            }
        }
    }
}
