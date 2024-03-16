using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Friendship
{
    public class FriendshipRetrievalException : FriendshipManagerException
    {
        public FriendshipRetrievalException()
        {
        }

        public FriendshipRetrievalException(string message)
            : base(message)
        {
        }

        public FriendshipRetrievalException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
