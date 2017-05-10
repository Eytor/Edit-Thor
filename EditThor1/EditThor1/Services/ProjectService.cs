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

        private string _userId = HttpContext.Current.User.Identity.GetUserId();

        public void AddProject(string name)
        {
            Project adds = new Project();
            File file = new File();
            adds.name = name;
            adds.ownerID = _userId;
            adds.ownerName = (from u in _db.Users
                          where u.Id == _userId
                          select u.UserName).SingleOrDefault();
            _db.Projects.Add(adds);
            _db.SaveChanges();

            var theProjectID = (from i in _db.Projects
                                where i.name == name
                                where i.ownerID == _userId
                                select i.ID).SingleOrDefault();
            file.name = "Index.html";
            file.typeID = 1;
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


        public bool isOwner(int projectID)
        {
            var result = (from p in _db.Projects
                          where p.ID == projectID
                          where p.ownerID == _userId
                          select p).SingleOrDefault();
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public bool HasAccess(int projectID)
        {
            var result = (from u in _db.UserProjects
                          where u.ProjectID == projectID
                          where u.UserID == _userId
                          select u).SingleOrDefault();
            if (result != null || isOwner(projectID))
            {
                return true;
            }
            return false;
        }

        public void LeaveProject(int projectID)
        {
            UserProject result = (from u in _db.UserProjects
                                  where u.ProjectID == projectID
                                  where u.UserID == _userId
                                  select u).FirstOrDefault();
            _db.UserProjects.Remove(result);
            _db.SaveChanges();
        }


        public bool checkSameName(string name)
        {
            List<Project> projectNames = (from f in _db.Projects
                                          where f.ownerID == _userId
                                          select f).ToList();
            foreach (var f in projectNames)
            {
                if (name == f.name)
                {
                    return true;
                }
            }
            return false;
        }
        public bool UserHasAccess(string userID, int projectID)
        {
            UserProject result = (from u in _db.UserProjects
                                  where u.ProjectID == projectID
                                  where u.UserID == userID
                                  select u).FirstOrDefault();
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public List<string> UserListofSharedProject(int projectID)
        {
            List<string> userNames = new List<string>();
            string ownerName = (from p in _db.Projects
                                where p.ID == projectID
                                select p.ownerName).SingleOrDefault();
            userNames.Add(ownerName);
            List<string> sharedUsers = (from p in _db.UserProjects
                                        join u in _db.Users on p.UserID equals u.Id
                                        where p.ProjectID == projectID
                                        select u.UserName).ToList();
            foreach (string name in sharedUsers)
            {
                userNames.Add(name);
            }
            
            return userNames;
        }
        public bool IsRegisteredUser(string name)
        {
            var userID = GetUserID(name);
            if (userID == null)
            {
                return false;
            }
            return true;
        }
    }
}