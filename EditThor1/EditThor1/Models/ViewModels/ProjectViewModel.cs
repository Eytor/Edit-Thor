using EditThor1.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EditThor1.Models.ViewModels
{
    public class ProjectViewModel
    {
        public int ID { get; set; }

        //The name of the project.
        [Required]
        [Display(Name = "Project name")]
        public string Name { get; set; }

        //The type of the project.
        public string OwnerID { get; set; }

        //The path to the file.
        public List<ApplicationUser> Userslist { get; set; }

        public List<File> ListFiles { get; set; }
    }
}