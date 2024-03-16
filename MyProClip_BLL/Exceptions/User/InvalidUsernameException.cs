using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.User
{
    public class InvalidUsernameException : UserManagerException
    {
        public InvalidUsernameException()
        {
        }

        public InvalidUsernameException(string message)
            : base(message)
        {
        }

        public InvalidUsernameException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
