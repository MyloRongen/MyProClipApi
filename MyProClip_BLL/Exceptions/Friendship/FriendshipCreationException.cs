using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Friendship
{
    public class FriendshipCreationException : FriendshipManagerException
    {
        public FriendshipCreationException()
        {
        }

        public FriendshipCreationException(string message)
            : base(message)
        {
        }

        public FriendshipCreationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
