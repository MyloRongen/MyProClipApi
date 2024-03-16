using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Friendship
{
    public class InvalidFriendshipDataException : FriendshipManagerException
    {
        public InvalidFriendshipDataException()
        {
        }

        public InvalidFriendshipDataException(string message)
            : base(message)
        {
        }

        public InvalidFriendshipDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
