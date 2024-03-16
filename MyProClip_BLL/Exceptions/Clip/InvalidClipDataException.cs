using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Clip
{
    public class InvalidClipDataException : ClipManagerException
    {
        public InvalidClipDataException()
        {
        }

        public InvalidClipDataException(string message)
            : base(message)
        {
        }

        public InvalidClipDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
