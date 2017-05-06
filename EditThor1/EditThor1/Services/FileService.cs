using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EditThor1.Models.Entities;
using EditThor1.Models;
using Microsoft.AspNet.Identity;

namespace EditThor1.Services
{
    public class FileService
    {

        private ApplicationDbContext _db = new ApplicationDbContext();

        public void SaveFile(byte[] arr, int id)
        {
            Project adds = new Project();
            File file = new File();

            file = (from f in _db.Files
                    where f.ID == id
                    select f).SingleOrDefault();
       
            file.file = arr;
            _db.SaveChanges();
        }

        public void DeleteFiles(int? projectId)
        {
            List<File> files = _db.Files.Where(x => x.projectID == projectId).ToList();
            if(files != null)
            {
                foreach(File file in files)
                {
                    _db.Files.Remove(file);
                }
                _db.SaveChanges();
            }
        }
    }
}