using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Message
{
    public class MessageCreationException : MessageManagerException
    {
        public MessageCreationException()
        {
        }

        public MessageCreationException(string message)
            : base(message)
        {
        }

        public MessageCreationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
