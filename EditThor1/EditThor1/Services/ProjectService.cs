using EditThor1.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditThor1.Services
{
    public class ProjectService
    {
        private List<Project> projects = new List<Project>();
        private Project Adds = new Project();

        public void AddProject(int ID, string name, string owner)
        {
            
            Adds.ID = ID;
            Adds.name = name;
            Adds.ownerID = owner;

            projects.Add(Adds);
           
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