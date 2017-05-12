using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EditThor1.Models.ViewModels
{
    public class ThemeViewModel
    {
        [Required]
        [Display(Name = "Theme")]
        public int ThemeID { get; set; }

        public string ThemeName { get; set; }

        public List<SelectListItem> ThemeList { get; set; }
    }
}