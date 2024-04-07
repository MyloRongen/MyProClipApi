using Microsoft.AspNetCore.Identity;
using MyProClip_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Interfaces.Services
{
    public interface IUserService
    {
        Task<ApplicationUser?> FindUserByNameAsync(string username);
        Task UserReportClip(ReportUserClip reportUserClip);
    }
}
