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
        private List<Project> projects = new List<Project>();
        
        private ApplicationDbContext _db = new ApplicationDbContext();

        public void AddProject(string name)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            Project Adds = new Project();
            Adds.name = name;
            Adds.ownerID = userId;

            _db.Projects.Add(Adds);
            _db.SaveChanges();
           
        }

        public Project GetProjectById(int id)
        {
            var result = (from project in projects
                          where project.ID == id
                          select project).SingleOrDefault();

            return result;
        }

        public IEnumerator<Project> GetAllUserProjects(int id)
        {
            IEnumerable<Project> result = from project in projects
                                          orderby project.name descending
                                          select project;
            return null;
        }
        
        
    }
}