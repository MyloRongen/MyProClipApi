using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Friendship
{
    public class InvalidFriendIdException : FriendshipManagerException
    {
        public InvalidFriendIdException()
        {
        }

        public InvalidFriendIdException(string message)
            : base(message)
        {
        }

        public InvalidFriendIdException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
