using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Friendship
{
    public class FriendshipDeletionException : FriendshipManagerException
    {
        public FriendshipDeletionException()
        {
        }

        public FriendshipDeletionException(string message)
            : base(message)
        {
        }

        public FriendshipDeletionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
