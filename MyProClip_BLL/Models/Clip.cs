using MyProClip_BLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MyProClip_BLL.Models
{
    public class Clip
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public PrivacyType Privacy { get; set; }

        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
