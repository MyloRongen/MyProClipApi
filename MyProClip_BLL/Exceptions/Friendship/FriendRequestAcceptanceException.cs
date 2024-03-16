using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Friendship
{
    public class FriendRequestAcceptanceException : FriendshipManagerException
    {
        public FriendRequestAcceptanceException()
        {
        }

        public FriendRequestAcceptanceException(string message)
            : base(message)
        {
        }

        public FriendRequestAcceptanceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
