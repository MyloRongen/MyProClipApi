﻿using Microsoft.AspNetCore.Identity;
using MyProClip_BLL.Models;

namespace MyProClip.Models
{
    public class ReportUserClipViewModel
    {
        public string UserId { get; set; }
        public string ReporterId { get; set; }

        public int ClipId { get; set; }

        public string Reason { get; set; }
    }
}