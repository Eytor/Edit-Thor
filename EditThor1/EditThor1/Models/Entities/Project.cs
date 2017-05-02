using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditThor1.Models.Entities
{
    public class Project
    {
        //ID of the project
        public int ID { get; set; }

        //the name of the project
        public string name { get; set; }

        //the type of the project
        public string type { get; set; }

        //the path to the file
        public string path { get; set; }

    }
}