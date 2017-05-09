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
            adds.ownerName = (from u in _db.Users
                          where u.Id == userId
                          select u.UserName).SingleOrDefault();
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
            var result = (from project in _db.Projects
                          where project.ID == id
                          select project).SingleOrDefault();

            return result;
        }

        public string GetProjectName(int id)
        {
            var result = (from project in _db.Projects
                          where project.ID == id
                          select project.name).SingleOrDefault();
            return result;
        }
        public List<Project> GetAllUserProjects()
        {
            List<Project> projects = GetMyProjects();
            List<Project> sharedProjects = GetSharedProjects();
            if (sharedProjects != null)
            {
                foreach (Project project in sharedProjects)
                {
                    projects.Add(project);
                }
            }
            
            return projects;
        }

        public List<Project> GetMyProjects()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            List<Project> result = (from project in _db.Projects
                                    orderby project.name ascending
                                    where userId == project.ownerID
                                    select project).ToList();

            return result;

        }

        public List<Project> GetSharedProjects()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            List<Project> projects = (from u in _db.UserProjects
                                      join i in _db.Projects on u.ProjectID equals i.ID
                                      where u.UserID == userId
                                      select i).ToList();
            return projects; 
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

        public bool ProjectExists(int? id)
        {
            if(_db.Projects.Where(x => x.ID == id).SingleOrDefault() != null)
            {
                return true;
            }
            return false;
        }
        public void DeleteProject(int? id)
        {
            Project project = _db.Projects.Where(x => x.ID == id).SingleOrDefault();
            if(project != null)
            {
                List<UserProject> connections = _db.UserProjects.Where(x => x.ProjectID == id).ToList();
                foreach (UserProject connection in connections)
                {
                    _db.UserProjects.Remove(connection);
                    _db.SaveChanges();
                }
                FileService fileService = new FileService();
                fileService.DeleteFiles(id);
                _db.Projects.Remove(project);
                _db.SaveChanges();
            }
        }

        public string GetUserID(string name)
        {
            ApplicationUser user = _db.Users.Where(x => x.UserName == name).SingleOrDefault();
            if(user != null)
            {
                return user.Id;
            }
            return "";
        }

        public void ShareProject(string userID, int projectID)
        {
            UserProject shareTable = new UserProject();
            shareTable.ProjectID = projectID;
            shareTable.UserID = userID;

            _db.UserProjects.Add(shareTable);
            _db.SaveChanges();
        }

        

    }
}