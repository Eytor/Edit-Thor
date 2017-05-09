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

        //the name of the project
        [Required]
        [StringLength(200, ErrorMessage = "The name must be at least {2} characters long.", MinimumLength = 1)]

        [Display(Name = "Project name")]
        public string name { get; set; }

        //the type of the project
        public string ownerID { get; set; }

        //the path to the file
        public List<ApplicationUser> userslist { get; set; }

        public List<File> listFiles { get; set; }
    }
}