using MyProClip_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Interfaces.Repositories
{
    public interface IClipRepository
    {
        Task<List<Clip>> GetClipsByUserId(string userId);
        Task<List<Clip>> GetPublicClips();
        void AddClip(Clip clip);
        Task<Clip?> GetClipById(int clipId);
        Task DeleteClipAsync(Clip clip);
    }
}
