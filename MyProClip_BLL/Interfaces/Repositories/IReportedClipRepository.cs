using MyProClip_BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Interfaces.Repositories
{
    public interface IReportedClipRepository
    {
        Task<List<ReportUserClip>> GetReportedClips();
    }
}
