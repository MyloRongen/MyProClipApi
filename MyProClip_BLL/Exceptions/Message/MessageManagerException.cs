using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Message
{
    public class MessageManagerException : Exception
    {
        public MessageManagerException()
        {
        }

        public MessageManagerException(string message)
            : base(message)
        {
        }

        public MessageManagerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
