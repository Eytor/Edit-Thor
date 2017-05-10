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
        public int projectId { get; set; }
        public int fileId { get; set; }
        public List<File> AllFiles { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public List<string> Users { get; set; }

        public string filetype { get; set; }

        public string  theme { get; set; }
    }
}