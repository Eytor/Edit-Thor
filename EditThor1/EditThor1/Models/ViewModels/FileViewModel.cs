﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditThor1.Models.ViewModels
{
    public class FileViewModel
    {
        //the ID of the file
        public int ID { get; set; }
        
        // project ID
        public int projectID { get; set; }

        //the name of the file
        public string name { get; set; }

        //the type of file ?
        public string type { get; set; }
    }
}