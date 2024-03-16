using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Friendship
{
    public class FriendshipManagerException : Exception
    {
        public FriendshipManagerException()
        {
        }

        public FriendshipManagerException(string message)
            : base(message)
        {
        }

        public FriendshipManagerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
