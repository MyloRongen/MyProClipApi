using Microsoft.AspNetCore.Identity;
using MyProClip_BLL.Exceptions.User;
using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser?> FindUserByNameAsync(string username)
        {
            try
            {
                IdentityUser? user = await _userManager.FindByNameAsync(username);
                return user;
            }
            catch (Exception ex)
            {
                throw new UserRetrievalException("Error retrieving user.", ex);
            }
        }
    }
}
