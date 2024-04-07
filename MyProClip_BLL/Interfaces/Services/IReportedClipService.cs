using MyProClip_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Interfaces.Services
{
    public interface IReportedClipService
    {
        Task<List<ReportUserClip>> GetReportedClips();
        Task<ReportUserClip?> GetReportUserClipById(int reportedClipId);
        Task DeleteReportedClip(ReportUserClip reportUserClip);
    }
}
