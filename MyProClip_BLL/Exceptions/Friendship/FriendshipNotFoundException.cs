using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Friendship
{
    public class FriendshipNotFoundException : FriendshipManagerException
    {
        public FriendshipNotFoundException()
        {
        }

        public FriendshipNotFoundException(string message)
            : base(message)
        {
        }

        public FriendshipNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
