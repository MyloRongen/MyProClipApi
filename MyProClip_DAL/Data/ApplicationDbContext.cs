using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyProClip_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
         base(options)
        { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Clip> Clips { get; set; }
        public DbSet<FriendShip> Friendships { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ReportUserClip> ReportUserClips { get; set; }
    }
}
