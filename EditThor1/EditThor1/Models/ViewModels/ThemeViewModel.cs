using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EditThor1.Models.ViewModels
{
    public class ThemeViewModel
    {
        public int themeID { get; set; }

        public string themeName { get; set; }

        public List<SelectListItem> themeList { get; set; }
    }
}