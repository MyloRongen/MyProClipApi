﻿using Microsoft.AspNetCore.Identity;
using MyProClip_BLL.Enums;
using MyProClip_BLL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProClip.Models
{
    public class ClipViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? UserName { get; set; }

        [NotMapped]
        public string? ThumbnailSrc { get; set; }

        [NotMapped]
        public string? VideoSrc { get; set; }

        public PrivacyType Privacy { get; set; }

        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
