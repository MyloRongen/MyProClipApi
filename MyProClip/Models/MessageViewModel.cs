using Microsoft.AspNetCore.Identity;
using MyProClip_BLL.Models;

namespace MyProClip.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public IdentityUser Sender { get; set; }

        public string ReceiverId { get; set; }
        public IdentityUser Receiver { get; set; }

        public int? ClipId { get; set; }
        public ClipViewModel? Clip { get; set; }

        public string Content { get; set; }

        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
