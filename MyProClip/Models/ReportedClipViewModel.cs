using MyProClip_BLL.Models;

namespace MyProClip.Models
{
    public class ReportedClipViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public ClipViewModel Clip { get; set; }
        public string Reason { get; set; }
    }
}
