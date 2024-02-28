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
    }
}
