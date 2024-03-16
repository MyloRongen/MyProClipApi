using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Friendship
{
    public class InvalidUserIdException : FriendshipManagerException
    {
        public InvalidUserIdException()
        {
        }

        public InvalidUserIdException(string message)
            : base(message)
        {
        }

        public InvalidUserIdException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
