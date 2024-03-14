using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Services
{
    public class ClipService : IClipService
    {
        private readonly IClipRepository _clipRepository;

        public ClipService(IClipRepository clipRepository)
        {
            _clipRepository = clipRepository;
        }

        public async Task<List<Clip>> GetClipsByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID does not exist");
            }

            return await _clipRepository.GetClipsByUserId(userId);
        }

        public async Task<List<Clip>> GetPublicClips()
        {
            return await _clipRepository.GetPublicClips();
        }

        public void AddClip(Clip clip)
        {
            if (string.IsNullOrWhiteSpace(clip.UserId) || string.IsNullOrWhiteSpace(clip.Title) || string.IsNullOrWhiteSpace(clip.VideoUrl) || string.IsNullOrWhiteSpace(clip.ThumbnailUrl))
            {
                throw new ArgumentException("Invalid project data. UserId, Title, video url and thumbnail url are required.");
            }

            _clipRepository.AddClip(clip);
        }

        public async Task<Clip?> GetClipById(int clipId)
        {
            if (clipId <= 0)
            {
                throw new Exception("The clip id couldn't be found!");
            }

            return await _clipRepository.GetClipById(clipId);
        }

        public async Task DeleteClipAsync(Clip clip)
        {
            if (clip == null)
            {
                throw new Exception("The clip couldn't be found!");
            }

            await _clipRepository.DeleteClipAsync(clip);
        }
    }
}
