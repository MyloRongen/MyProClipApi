using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.User
{
    public class UserRetrievalException : UserManagerException
    {
        public UserRetrievalException()
        {
        }

        public UserRetrievalException(string message)
            : base(message)
        {
        }

        public UserRetrievalException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
