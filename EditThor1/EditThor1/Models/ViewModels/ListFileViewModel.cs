using EditThor1.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EditThor1.Models.ViewModels
{
    public class ListFileViewModel
    {
        public int ProjectId { get; set; }

        public int FileId { get; set; }

        public List<File> AllFiles { get; set; }

        [AllowHtml]
        public string Content { get; set; }
        
        public List<string> Users { get; set; }

        public string Filetype { get; set; }

        public string  Theme { get; set; }
    }
}