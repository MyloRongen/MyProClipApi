using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Friendship
{
    public class PendingFriendRequestsRetrievalException : FriendshipManagerException
    {
        public PendingFriendRequestsRetrievalException()
        {
        }

        public PendingFriendRequestsRetrievalException(string message)
            : base(message)
        {
        }

        public PendingFriendRequestsRetrievalException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
