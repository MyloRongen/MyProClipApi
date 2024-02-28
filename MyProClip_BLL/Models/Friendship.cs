using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyProClip_BLL.Enums;

namespace MyProClip_BLL.Models
{
    public class FriendShip
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public string FriendId { get; set; }
        public IdentityUser Friend { get; set; }

        public FriendshipStatus Status { get; set; }
    }
}
