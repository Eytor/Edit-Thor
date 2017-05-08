using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EditThor1.Models.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "Email")]
        public string userName { get; set; }

        public string ID { get; set; }

        public int ProjectID { get; set; }
    }
}