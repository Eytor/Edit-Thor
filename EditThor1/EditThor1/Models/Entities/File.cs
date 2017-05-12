using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EditThor1.Models.Entities
{
    public class File
    {
        // The ID of the file.
        public int ID { get; set; }

        // The name of the file.
        public string Name { get; set; }

        // The type of file.
        public int TypeID { get; set; }

        // The actual file itself.
        [AllowHtml]
        public byte[] TheFile { get; set; }

        // The ID of the project this particular file belongs to.
        public int ProjectID { get; set; }

    }
}