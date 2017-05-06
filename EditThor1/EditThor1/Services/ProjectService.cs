using EditThor1.Models;
using EditThor1.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Security;
namespace EditThor1.Services
{
    public class ProjectService
    {
        private List<Project> _projects = new List<Project>();
        
        private ApplicationDbContext _db = new ApplicationDbContext();

        public void AddProject(string name)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            Project adds = new Project();
            File file = new File();
            adds.name = name;
            adds.ownerID = userId;

            _db.Projects.Add(adds);
            _db.SaveChanges();
            var theProjectID = (from i in _db.Projects
                                where i.name == name
                                where i.ownerID == userId
                                select i.ID).SingleOrDefault();
            file.name = "Index.html";
            file.type = "HTML";
            file.file = new byte[64];
            file.projectID = theProjectID;
            _db.Files.Add(file);
            _db.SaveChanges();
        }

        public Project GetProjectById(int id)
        {
            var result = (from project in _projects
                          where project.ID == id
                          select project).SingleOrDefault();

            return result;
        }

        public List<Project> GetAllUserProjects()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            List<Project> result = (from project in _db.Projects
                                           orderby project.name ascending
                                           where userId == project.ownerID
                                           select project).ToList();

            return result;

        }

        public List<File> OpenProject(int? id)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            Project projectId = new Project();
            List<File> result = (from i in _db.Files
                                 where i.projectID == id
                                 select i).ToList();
            return result;
        }

        public List<File> GetAllFiles(int id)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            Project projectId = GetProjectById(id);
            List<File> result = (from i in _db.Files
                                 where i.ID == id
                                 select i).ToList();
            return result;
        }

    }
}