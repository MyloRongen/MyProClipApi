using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyProClip_BLL.Exceptions.User;
using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_BLL.Models;
using MyProClip_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProClip_BLL.Exceptions.Clip;

namespace MyProClip_DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<ApplicationUser?> FindUserByNameAsync(string username)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByNameAsync(username);
                return user;
            }
            catch (Exception ex)
            {
                throw new UserRetrievalException("Error retrieving user.", ex);
            }
        }

        public async Task UserReportClip(ReportUserClip reportUserClip)
        {
            try
            {
                _dbContext.ReportUserClips.Add(reportUserClip);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new UserReportClipException("Error reporting user clip.", ex);
            }
        }
    }
}
