using MyProClip_BLL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProClip.Models
{
    public class ClipViewModelRequest
    {
        public string Title { get; set; }

        [NotMapped]
        public IFormFile ThumbnailFile { get; set; }

        [NotMapped]
        public IFormFile VideoClipFile { get; set; }
    }
}
