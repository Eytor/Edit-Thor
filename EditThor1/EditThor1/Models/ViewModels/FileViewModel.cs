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
        //The ID of the file.
        public int ID { get; set; }
        
        // Project ID.
        public int ProjectID { get; set; }

        //The name of the file.
        [Required]
        [Display(Name = "File name")]
        public string Name { get; set; }

        //The type of file.
        public List<SelectListItem> Type { get; set; }

        [Required]
        [Display(Name = "File type")]
        public int TypeID { get; set; }
    }
}