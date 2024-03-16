using Microsoft.AspNetCore.Identity;
using MyProClip_BLL.Exceptions.User;
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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IdentityUser?> FindUserByNameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new InvalidUsernameException("The username was empty!");
            }

            return await _userRepository.FindUserByNameAsync(username);
        }
    }
}
