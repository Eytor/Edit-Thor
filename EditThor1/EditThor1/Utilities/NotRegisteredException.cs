using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditThor1.Utilities
{
    public class NotRegisteredException : Exception
    {
        public NotRegisteredException()
        : base(String.Format("User not registered.")) { }

        public NotRegisteredException(string message) : base(message)
        {
        }
    }
}