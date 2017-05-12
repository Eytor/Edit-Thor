using EditThor1.Models;
using EditThor1.Models.Entities;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditThor1.Services
{
    public class ProjectService
    {
        private readonly IAppDataContext db;

        public ProjectService(IAppDataContext dbContext)
        {
           db = dbContext ?? new ApplicationDbContext();
        }

        public ProjectService()
        {
            _db = new ApplicationDbContext();

        }

        private ApplicationDbContext _db = new ApplicationDbContext();

        private List<Project> _projects = new List<Project>();
       
        private string _userId = HttpContext.Current.User.Identity.GetUserId();
        // Creates project with 1 empty html dummy file.
        public void AddProject(string name)
        {
            // Project
            Project adds = new Project();
            adds.Name = name;
            adds.OwnerID = _userId;
            adds.OwnerName = (from u in _db.Users
                          where u.Id == _userId
                          select u.UserName).SingleOrDefault();
            _db.Projects.Add(adds);
            _db.SaveChanges();
            // Gets id of project we just created, since we don't allow projects with same name and same owner this isn't a problem.
            var theProjectID = (from i in _db.Projects
                                where i.Name == name
                                where i.OwnerID == _userId
                                select i.ID).SingleOrDefault();
            // Dummy Html file
            File file = new File();
            file.Name = "Index.html";
            file.TypeID = 1;
            file.TheFile = new byte[64];
            file.ProjectID = theProjectID;
            _db.Files.Add(file);
            _db.SaveChanges();
        }
        // Gets project from database by id and returns it.
        public Project GetProjectById(int projectID)
        {
            var result = (from project in _db.Projects
                          where project.ID == projectID
                          select project).SingleOrDefault();

            return result;
        }
        // Gets project name from database by id and returns it.
        public string GetProjectName(int projectID)
        {
            var result = (from project in _db.Projects
                          where project.ID == projectID
                          select project.Name).SingleOrDefault();
            return result;
        }
        // Gets a list of all projects that the user is associated with and returns it.
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
        // Gets a list of all projects owned by user and returns it.
        public List<Project> GetMyProjects()
        {
            List<Project> result = (from project in _db.Projects
                                    orderby project.Name ascending
                                    where project.OwnerID == _userId
                                    select project).ToList();
            return result;
        }
        // Gets a list of all projects shared with current user and returns it.
        public List<Project> GetSharedProjects()
        {
            List<Project> projects = (from u in _db.UserProjects
                                      join i in _db.Projects on u.ProjectID equals i.ID
                                      where u.UserID == _userId
                                      select i).ToList();
            return projects; 
        }
        // Gets a list of all files within project and returns it.
        public List<File> GetAllFiles(int projectID)
        {
            List<File> result = (from i in _db.Files
                                 where i.ProjectID == projectID
                                 select i).ToList();
            return result;
        }
        // Checks if project exists and returns true if it exists and false otherwise.
        public bool ProjectExists(int? projectID)
        {
            if (_db.Projects.Where(x => x.ID == projectID).SingleOrDefault() != null)
            {
                return true;
            }
            return false;
        }
        // Deletes Project, all user connections associated with that project and files by project id.
        public void DeleteProject(int? projectID)
        {
            Project project = _db.Projects.Where(x => x.ID == projectID).SingleOrDefault();
            if(project != null)
            {
                List<UserProject> connections = _db.UserProjects.Where(x => x.ProjectID == projectID).ToList();
                foreach (UserProject connection in connections)
                {
                    _db.UserProjects.Remove(connection);
                    _db.SaveChanges();
                }
                FileService fileService = new FileService();
                fileService.DeleteFiles(projectID);
                _db.Projects.Remove(project);
                _db.SaveChanges();
            }
        }
        // Get users id by username (email since it's automatically set as username).
        public string GetUserID(string name)
        {
            ApplicationUser user = _db.Users.Where(x => x.UserName == name).SingleOrDefault();
            if (user != null)
            {
                return user.Id;
            }
            return "";
        }
        // Share project with user by user id and project id.
        public void ShareProject(string userID, int projectID)
        {
            UserProject shareTable = new UserProject();
            shareTable.ProjectID = projectID;
            shareTable.UserID = userID;

            _db.UserProjects.Add(shareTable);
            _db.SaveChanges();
        }

        // Checks if current user has access to a project by project id. Used when opening a project.
        public bool HasAccess(int projectID)
        {
            return UserHasAccess(_userId, projectID);
        }
        // Removes access for current user in project by project id.
        public void LeaveProject(int projectID)
        {
            UserProject result = (from u in _db.UserProjects
                                  where u.ProjectID == projectID
                                  where u.UserID == _userId
                                  select u).FirstOrDefault();
            _db.UserProjects.Remove(result);
            _db.SaveChanges();
        }
        // Checks if user already has project with same name.
        public bool checkSameName(string name)
        {
            List<Project> projectNames = (from f in _db.Projects
                                          where f.OwnerID == _userId
                                          select f).ToList();
            foreach (var f in projectNames)
            {
                if (name == f.Name)
                {
                    return true;
                }
            }
            return false;
        }
        // Checks if project can be shared with the specified user.
        public bool UserHasAccess(string userID, int projectID)
        {
            string result = (from u in _db.UserProjects
                            where u.ProjectID == projectID
                            select u.UserID).FirstOrDefault();
            string ownerID = (from p in _db.Projects
                             where p.ID == projectID
                             select p.OwnerID).SingleOrDefault();
            if (result == userID || ownerID == userID )
            {
                return true;
            }
            return false;
        }
        // Creates list of all users with access to project by project id and returns it
        public List<string> UserListofSharedProject(int projectID)
        {
            List<string> userNames = new List<string>();
            string ownerName = (from p in _db.Projects
                                where p.ID == projectID
                                select p.OwnerName).SingleOrDefault();
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
        // Checks if user is registered returns false otherwise
        public bool IsRegisteredUser(string name)
        {
            var userID = GetUserID(name);
            if (userID == "")
            {
                return false;
            }
            return true;
        }
        //check if current user is owner of project
        public bool IsOwnerOfProject(int projectID)
        {
            string ownerID = (from p in _db.Projects
                              where p.ID == projectID
                              select p.OwnerID).SingleOrDefault();
            if (ownerID == _userId)
            {
                return true;
            }

            return false;
        }
    }
}