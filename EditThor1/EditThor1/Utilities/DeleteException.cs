using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditThor1.Utilities
{
    public class DeleteException : Exception
    {
        public DeleteException()
        : base(String.Format("Project could not be deleted.")) { }

        public DeleteException(string message) : base(message)
        {
        }
    }
}