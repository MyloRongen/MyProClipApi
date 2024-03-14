using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Interfaces.Services
{
    public interface IUserService
    {
        Task<IdentityUser?> FindUserByNameAsync(string username);
    }
}
