using EditThor1.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EditThor1.Models.ViewModels
{
    public class ListFileViewModel
    {
        public int projectId { get; set; }
        public int fileId { get; set; }
        public List<File> AllFiles { get; set; }
        public string Content { get; set; }
        public List<string> Users { get; set; }
    }
}