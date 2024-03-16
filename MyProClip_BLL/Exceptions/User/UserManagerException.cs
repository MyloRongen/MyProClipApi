using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.User
{
    public class UserManagerException : Exception
    {
        public UserManagerException()
        {
        }

        public UserManagerException(string message)
            : base(message)
        {
        }

        public UserManagerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
