using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EditThor1.Models.Entities;
using EditThor1.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

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

        public void CreateFile(int projectID, string name, int type)
        {
            File file = new File();
            file.name = name + GetFileEnding(type);
            file.typeID = type;
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

        public List<SelectListItem> GetAvailableTypes()
        {
            List<SelectListItem> categories = new List<SelectListItem>();

            categories.Add(new SelectListItem() { Value = "", Text = " - Choose a File Type - " });

            _db.FileTypes.ToList().ForEach((x) =>
            {
                categories.Add(new SelectListItem() { Value = x.ID.ToString(), Text = x.typeName });
            });
            
            return categories;
        }

        public string GetFileEnding(int typeID)
        {
            string fileEnding = (from t in _db.FileTypes
                                 where t.ID == typeID
                                 select t.fileEnding).SingleOrDefault();
            return fileEnding;
        }

        public string GetFileTypeName(int fileID)
        {
            int typeID = (from f in _db.Files
                          join t in _db.FileTypes on f.typeID equals t.ID
                          where f.ID == fileID
                          select t.ID).SingleOrDefault();
            string typename = (from t in _db.FileTypes
                              where t.ID == typeID
                              select t.typeName).SingleOrDefault();
            return typename.ToLower();
        }
    }
}