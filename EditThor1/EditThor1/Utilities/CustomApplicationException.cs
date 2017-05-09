using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditThor1.Utilities
{
    public class CustomApplicationException : Exception
    {
        public CustomApplicationException()
        : base(String.Format("Something bad happened.")) { }
    }
}