using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Message
{
    public class InvalidMessageDataException : MessageManagerException
    {
        public InvalidMessageDataException()
        {
        }

        public InvalidMessageDataException(string message)
            : base(message)
        {
        }

        public InvalidMessageDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
