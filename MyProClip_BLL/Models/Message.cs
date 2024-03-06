using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public IdentityUser Sender { get; set; }

        public string ReceiverId { get; set; }
        public IdentityUser Receiver { get; set; }

        public string? ClipLink { get; set; }
        public string Content { get; set; }

        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
