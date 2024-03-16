using MyProClip_BLL.Exceptions.Clip;
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
                throw new ArgumentException("User ID is required.", nameof(userId));
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
                throw new InvalidClipDataException("Invalid clip data. UserId, Title, video URL, and thumbnail URL are required.");
            }

            _clipRepository.AddClip(clip);
        }

        public async Task<Clip?> GetClipById(int clipId)
        {
            if (clipId <= 0)
            {
                throw new ArgumentException("Invalid clip ID.", nameof(clipId));
            }

            return await _clipRepository.GetClipById(clipId);
        }

        public async Task DeleteClipAsync(Clip clip)
        {
            if (clip == null)
            {
                throw new ArgumentNullException(nameof(clip), "Clip instance is null.");
            }

            await _clipRepository.DeleteClipAsync(clip);
        }
    }
}
