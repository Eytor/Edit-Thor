using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditThor1.Models.Entities
{
    public class FileTypes
    {
        //The ID of the filetype.
        public int ID { get; set; }

        //The type of the the file
        public string TypeName { get; set; }

        public string FileEnding { get; set; }
    }
}