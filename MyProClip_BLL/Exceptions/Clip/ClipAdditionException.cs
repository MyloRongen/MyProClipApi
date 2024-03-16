using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Clip
{
    public class ClipAdditionException : ClipManagerException
    {
        public ClipAdditionException()
        {
        }

        public ClipAdditionException(string message)
            : base(message)
        {
        }

        public ClipAdditionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
