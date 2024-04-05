using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Clip
{
    public class UserReportClipException : ClipManagerException
    {
        public UserReportClipException()
        {
        }

        public UserReportClipException(string message)
            : base(message)
        {
        }

        public UserReportClipException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
