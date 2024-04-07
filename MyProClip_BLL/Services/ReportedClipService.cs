using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProClip_BLL.Exceptions.Clip;
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

        public async Task<ReportUserClip?> GetReportUserClipById(int reportedClipId)
        {
            if (reportedClipId <= 0)
            {
                throw new UserReportClipException("Reported clip not found.");
            }

            return await _reportedClipRepository.GetReportUserClipById(reportedClipId);
        }

        public async Task DeleteReportedClip(ReportUserClip reportUserClip)
        {
            if (reportUserClip == null)
            {
                throw new ArgumentNullException(nameof(reportUserClip), "Clip instance is null.");
            }

            await _reportedClipRepository.DeleteReportedClip(reportUserClip);
        }
    }
}
