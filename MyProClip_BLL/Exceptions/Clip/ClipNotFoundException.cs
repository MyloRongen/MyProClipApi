using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Clip
{
    public class ClipNotFoundException : ClipManagerException
    {
        public ClipNotFoundException()
        {
        }

        public ClipNotFoundException(string message)
            : base(message)
        {
        }

        public ClipNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
