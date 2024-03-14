using Microsoft.AspNetCore.Mvc;
using MyProClip.Models;
using MyProClip_BLL.Models;

namespace MyProClip.Services
{
    public class ClipTransformer
    {
        public ClipViewModel? ClipToViewModel(Clip? clip)
        {
            if (clip == null)
            {
                return null;
            }
            else
            {
                string httpsPath = "https://localhost:8000";

                ClipViewModel newClip = new()
                {
                    Id = clip.Id,
                    Title = clip.Title,
                    UserName = clip.User.UserName,
                    Privacy = clip.Privacy,
                    ThumbnailSrc = String.Format("{0}/Thumbnails/{1}", httpsPath, clip.ThumbnailUrl),
                    VideoSrc = String.Format("{0}/Videos/{1}", httpsPath, clip.VideoUrl),
                    UpdatedAt = clip.UpdatedAt,
                    CreatedAt = clip.CreatedAt
                };

                return newClip;
            }      
        }
    }
}
