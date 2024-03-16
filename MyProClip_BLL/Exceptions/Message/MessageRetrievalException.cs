using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Message
{
    public class MessageRetrievalException : MessageManagerException
    {
        public MessageRetrievalException()
        {
        }

        public MessageRetrievalException(string message)
            : base(message)
        {
        }

        public MessageRetrievalException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
