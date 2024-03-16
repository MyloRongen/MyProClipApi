using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Clip
{
    public class ClipRetrievalException : ClipManagerException
    {
        public ClipRetrievalException()
        {
        }

        public ClipRetrievalException(string message)
            : base(message)
        {
        }

        public ClipRetrievalException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
