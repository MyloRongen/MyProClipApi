using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_BLL.Exceptions.Friendship
{
    public class FriendshipCheckException : FriendshipManagerException
    {
        public FriendshipCheckException()
        {
        }

        public FriendshipCheckException(string message)
            : base(message)
        {
        }

        public FriendshipCheckException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
