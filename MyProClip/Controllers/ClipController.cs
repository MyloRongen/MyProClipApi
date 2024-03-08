using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyProClip.Models;
using MyProClip_BLL.Enums;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using System.Security.Claims;

namespace MyProClip.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ClipController : ControllerBase
    {
        private readonly IClipService _clipService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClipController(IClipService clipService, IWebHostEnvironment webHostEnvironment)
        {
            _clipService = clipService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("get-public-clips")]
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
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve Clips: {ex.Message}");
            }
        }

        [HttpGet("get-clips")]
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
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve Clips: {ex.Message}");
            }
        }

        [HttpPost("add-clip")]
        public async Task<IActionResult> AddClip([FromForm] ClipViewModelRequest clipViewModelRequest)
        {
            try
            {
                if (clipViewModelRequest == null)
                {
                    return BadRequest("No information was given about the clip.");
                }

                string thumbnailUrl = await SaveImageAsync(clipViewModelRequest.ThumbnailFile);
                string videoClipUrl = await SaveVideoAsync(clipViewModelRequest.VideoClipFile);

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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("delete-clip/{clipId}")]
        public async Task<IActionResult> DeleteClip(int clipId)
        {
            try
            {
                Clip? clip = await _clipService.GetClipById(clipId);

                if (clip == null)
                {
                    return BadRequest("Clip doesn't exist in the current context.");
                }
                else
                {
                    if (clip.UserId != GetUserIdFromClaims())
                    {
                        return BadRequest("Clip doesn't belong to the user.");
                    }

                    await _clipService.DeleteClipAsync(clip);
                    DeleteVideo(clip.VideoUrl);
                }

                return Ok("Clip was successfully deleted!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private string GetUserIdFromClaims()
        {
            string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                throw new Exception("User ID not found or invalid.");
            }

            return userIdString;
        }


        [NonAction]
        public async Task<string> SaveImageAsync(IFormFile thumbnailFile)
        {
            Guid myuuid = Guid.NewGuid();

            string thumbnailName = new String(Path.GetFileNameWithoutExtension(thumbnailFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            thumbnailName = thumbnailName + DateTime.Now.ToString("yymmssfff") + myuuid.ToString() + Path.GetExtension(thumbnailFile.FileName);
            var thumbnailPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Thumbnails", thumbnailName);

            using (var fileStream = new FileStream(thumbnailPath, FileMode.Create))
            {
                await thumbnailFile.CopyToAsync(fileStream);
            }

            return thumbnailName;
        }


        [NonAction]
        public async Task<string> SaveVideoAsync(IFormFile videoFile)
        {
            Guid myuuid = Guid.NewGuid();

            string videoName = new String(Path.GetFileNameWithoutExtension(videoFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            videoName = videoName + DateTime.Now.ToString("yymmssfff") + myuuid.ToString() + Path.GetExtension(videoFile.FileName);
            var videoPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Videos", videoName);

            using (var fileStream = new FileStream(videoPath, FileMode.Create))
            {
                await videoFile.CopyToAsync(fileStream);
            }

            return videoName;
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
