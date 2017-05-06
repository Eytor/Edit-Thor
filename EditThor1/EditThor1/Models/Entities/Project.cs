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

        //the list of all users associated with the project
        public virtual List<ApplicationUser> userslist { get; set; }

        //the ID of the user who created the project
        public string ownerID { get; set; }

        //the list of all files in a project
        public virtual List<File> filesList { get; set; }

    }
}