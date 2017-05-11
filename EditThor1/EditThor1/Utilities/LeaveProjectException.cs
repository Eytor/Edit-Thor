using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditThor1.Utilities
{
    public class LeaveProjectException : Exception
    {
        public LeaveProjectException()
        : base(String.Format("Could not leave project.")) { }

        public LeaveProjectException(string message) : base(message)
        {
        }
    }
}