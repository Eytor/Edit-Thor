using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EditThor1.Models.Entities
{
    public class Project
    {
        //ID of the project
        public int ID { get; set; }

        //The name of the project.
        public string Name { get; set; }

        //The list of all users associated with the project.
        public virtual List<ApplicationUser> Userslist { get; set; }

        //The ID of the user who created the project.
        public string OwnerID { get; set; }

        public string OwnerName { get; set; }

        //The list of all files in a project.
        public virtual List<File> FilesList { get; set; }

    }
}