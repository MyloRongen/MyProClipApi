using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;

namespace MyProClip_BLL.Services
{
    public class ReportedClipService : IReportedClipService
    {
        private readonly IReportedClipRepository _reportedClipRepository;

        public ReportedClipService(IReportedClipRepository reportedClipRepository)
        {
            _reportedClipRepository = reportedClipRepository;
        }

        public async Task<List<ReportUserClip>> GetReportedClips()
        {
            return await _reportedClipRepository.GetReportedClips();
        }
    }
}
