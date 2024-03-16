using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Clip
{
    public class ClipManagerException : Exception
    {
        public ClipManagerException()
        {
        }

        public ClipManagerException(string message)
            : base(message)
        {
        }

        public ClipManagerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
