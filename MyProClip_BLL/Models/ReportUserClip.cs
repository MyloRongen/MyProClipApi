using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Models
{
    public class ReportUserClip
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string ReporterId { get; set; }
        public ApplicationUser Reporter { get; set; }

        public int ClipId { get; set; }
        public Clip Clip { get; set; }

        public string Reason { get; set; }
    }
}
