using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EditThor1.Models.Entities
{
    public class File
    {
        //the ID of the file
        public int ID { get; set; }

        //the name of the file
        public string name { get; set; }

        //the type of file ?
        public int typeID { get; set; }

        //the actual file itself
        [AllowHtml]
        public byte[] file { get; set; }

        //the ID of the project this particular file belongs to
        public int projectID { get; set; }

    }
}