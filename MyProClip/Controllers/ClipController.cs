using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyProClip.Models;
using MyProClip.Services;
using MyProClip_BLL.Enums;
using MyProClip_BLL.Exceptions.Clip;
using MyProClip_BLL.Exceptions.User;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using System.Security.Claims;

namespace MyProClip.Controllers
{
    [Route("api/")]
    [Authorize]
    [ApiController]
    public class ClipController : ControllerBase
    {
        private readonly IClipService _clipService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ImageService _imageService;
        private readonly VideoService _videoService;

        public ClipController(IClipService clipService, IWebHostEnvironment webHostEnvironment)
        {
            _clipService = clipService;
            _webHostEnvironment = webHostEnvironment;
            _imageService = new ImageService(webHostEnvironment);
            _videoService = new VideoService(webHostEnvironment);
        }

        [HttpGet("clips/public")]
        public async Task<IActionResult> GetPublicClips()
        {
            try
            {
                List<Clip> clips = await _clipService.GetPublicClips();
                List<ClipViewModel> clipViewModels = [];

                foreach (Clip clip in clips)
                {
                    ClipViewModel newClip = new()
                    {
                        Id = clip.Id,
                        Title = clip.Title,
                        UserName = clip.User.UserName,
                        Privacy = clip.Privacy,
                        ThumbnailSrc = String.Format("{0}://{1}{2}/Thumbnails/{3}", Request.Scheme, Request.Host, Request.PathBase, clip.ThumbnailUrl),
                        VideoSrc = String.Format("{0}://{1}{2}/Videos/{3}", Request.Scheme, Request.Host, Request.PathBase, clip.VideoUrl),
                        UpdatedAt = clip.UpdatedAt,
                        CreatedAt = clip.CreatedAt
                    };

                    clipViewModels.Add(newClip);
                }

                return Ok(clipViewModels);
            }
            catch (ClipManagerException ex)
            {
                return NotFound($"Failed to retrieve public clips: {ex.Message}");
            }       
        }

        [HttpGet("clips")]
        public async Task<IActionResult> GetClips()
        {
            try
            {
                string userId = GetUserIdFromClaims();

                List<Clip> clips = await _clipService.GetClipsByUserId(userId);

                List<ClipViewModel> clipViewModels = [];

                foreach (Clip clip in clips)
                {
                    ClipViewModel newClip = new()
                    {
                        Id = clip.Id,
                        Title = clip.Title,
                        UserName = clip.User.UserName,
                        Privacy = clip.Privacy,
                        ThumbnailSrc = String.Format("{0}://{1}{2}/Thumbnails/{3}", Request.Scheme, Request.Host, Request.PathBase, clip.ThumbnailUrl),
                        VideoSrc = String.Format("{0}://{1}{2}/Videos/{3}", Request.Scheme, Request.Host, Request.PathBase, clip.VideoUrl),
                        UpdatedAt = clip.UpdatedAt,
                        CreatedAt = clip.CreatedAt
                    };

                    clipViewModels.Add(newClip);
                }

                return Ok(clipViewModels);
            }
            catch (UserManagerException ex)
            {
                return NotFound($"Failed to retrieve user clips: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return NotFound($"Failed to retrieve user clips: {ex.Message}");
            }
            catch (ClipManagerException ex)
            {
                return NotFound($"Failed to retrieve user clips: {ex.Message}");
            }
        }

        [HttpPost("clips")]
        public async Task<IActionResult> AddClip([FromForm] ClipViewModelRequest clipViewModelRequest)
        {
            try
            {
                if (clipViewModelRequest == null)
                {
                    return NotFound("No information was given about the clip.");
                }

                string thumbnailUrl = await _imageService.SaveImageAsync(clipViewModelRequest.ThumbnailFile);
                string videoClipUrl = await _videoService.SaveVideoAsync(clipViewModelRequest.VideoClipFile);

                Clip newClip = new()
                {
                    UserId = GetUserIdFromClaims(),
                    Title = clipViewModelRequest.Title,
                    VideoUrl = videoClipUrl,
                    ThumbnailUrl = thumbnailUrl,
                    Privacy = PrivacyType.Private,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                _clipService.AddClip(newClip);

                return Ok("Clip added successfully");
            }
            catch (UserManagerException ex)
            {
                return NotFound($"Failed to add a clip: {ex.Message}");
            }
            catch (ClipManagerException ex)
            {
                return NotFound($"Failed to add a clip: {ex.Message}");
            }
        }

        [HttpDelete("clips/{clipId}")]
        public async Task<IActionResult> DeleteClip(int clipId)
        {
            try
            {
                Clip? clip = await _clipService.GetClipById(clipId);

                if (clip == null)
                {
                    return NotFound("Clip doesn't exist in the current context.");
                }
                else
                {
                    if (clip.UserId != GetUserIdFromClaims())
                    {
                        return NotFound("Clip doesn't belong to the user.");
                    }

                    await _clipService.DeleteClipAsync(clip);
                    DeleteVideo(clip.VideoUrl);
                }

                return Ok("Clip was successfully deleted!");
            }
            catch (UserManagerException ex)
            {
                return NotFound($"Failed to delete a clip: {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                return NotFound($"Failed to delete a clip: {ex.Message}");
            }
            catch (ClipManagerException ex)
            {
                return NotFound($"Failed to delete a clip: {ex.Message}");
            }
        }

        private string GetUserIdFromClaims()
        {
            string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UserRetrievalException("User ID not found or invalid.");
            }

            return userIdString;
        }

        [NonAction]
        public void DeleteVideo(string videoName)
        {
            var videoPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Videos", videoName);

            if (System.IO.File.Exists(videoPath))
            {
                System.IO.File.Delete(videoPath);
            }
        }
    }
}
