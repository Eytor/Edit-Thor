using EditThor1.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EditThor1.Models.ViewModels
{
    public class FileViewModel
    {
        //the ID of the file
        public int ID { get; set; }
        
        // project ID
        public int projectID { get; set; }

        //the name of the file
        [Required]
        [Display(Name = "File name")]
        public string name { get; set; }

        //the type of file ?
        public List<SelectListItem> type { get; set; }
        [Required]
        [Display(Name = "File type")]
        public int typeID { get; set; }
    }
}