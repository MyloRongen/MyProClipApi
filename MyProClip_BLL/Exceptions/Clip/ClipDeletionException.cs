using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Clip
{
    public class ClipDeletionException : ClipManagerException
    {
        public ClipDeletionException()
        {
        }

        public ClipDeletionException(string message)
            : base(message)
        {
        }

        public ClipDeletionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
