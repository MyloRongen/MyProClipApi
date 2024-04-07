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
using MyProClip_BLL.Exceptions.Clip;

namespace MyProClip_BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApplicationUser?> FindUserByNameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new InvalidUsernameException("The username was empty!");
            }

            return await _userRepository.FindUserByNameAsync(username);
        }

        public async Task UserReportClip(ReportUserClip reportUserClip)
        {
            if (string.IsNullOrEmpty(reportUserClip.UserId))
            {
                throw new UserNotFoundException("User id does not exist");
            }

            if (string.IsNullOrEmpty(reportUserClip.ReporterId))
            {
                throw new UserNotFoundException("Reporter id does not exist");
            }

            if (reportUserClip.ClipId <= 0)
            {
                throw new ClipNotFoundException("Clip does not exist");
            }

            await _userRepository.UserReportClip(reportUserClip);
        }
    }
}
