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

        public void SaveFile(byte[] arr, int fileId)
        {
            Project adds = new Project();
            File file = new File();

            file = (from f in _db.Files
                    where f.ID == fileId
                    select f).SingleOrDefault();
       
            file.file = arr;
            _db.SaveChanges();
        }

        public byte[] GetFiles(int? id, int? prId)
        {
            byte[] arr;

            File data = new File();

            data = (from f in _db.Files
                    where f.ID == id
                    where f.projectID == prId
                    select f).SingleOrDefault();

            arr = data.file;
            return arr;
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

        public void CreateFile(int projectID, string name, string type)
        {
            File file = new File();
            file.name = name;
            file.type = type;
            file.projectID = projectID;
            file.file = new byte[0];
            _db.Files.Add(file);
            _db.SaveChanges();
        }
        public void DeleteFile(int fileID)
        {
            File file = _db.Files.Where(x => x.ID == fileID).SingleOrDefault();
            if (file != null)
            {
                _db.Files.Remove(file);
                _db.SaveChanges();
            }
        }
    }
}