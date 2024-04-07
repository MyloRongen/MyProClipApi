using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyProClip_BLL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Points { get; set; }
        public bool IsBanned { get; set; }
    }
}
