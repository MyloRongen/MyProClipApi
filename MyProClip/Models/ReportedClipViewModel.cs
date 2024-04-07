using MyProClip_BLL.Models;

namespace MyProClip.Models
{
    public class ReportedClipViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ReporterId { get; set; }
        public string Username { get; set; }
        public string ReporterName { get; set; }
        public ClipViewModel Clip { get; set; }
        public string Reason { get; set; }
    }
}
