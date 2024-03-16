using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.User
{
    public class UsernameUpdateException : UserManagerException
    {
        public UsernameUpdateException()
        {
        }

        public UsernameUpdateException(string message)
            : base(message)
        {
        }

        public UsernameUpdateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
