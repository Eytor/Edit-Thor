using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EditThor1.Models;
using System.Data.Entity;
using EditThor1.Models.Entities;
using System.Data.Entity.Infrastructure;
using EditThor1.Services;
using System.Web;
using System.Data.Entity;

namespace EditThor1.Tests
{
    /// <summary>
    /// Summary description for MockDataContext
    /// </summary>
    ///
    /*public class MockUserIdService : IUserIdService
    {
            // útfærir GetUserId() með því að kalla á HttpContext.Current.etc og skila því sem það skilar
            string IUserIdService.GetUserId()
            {
                var _userId = "jkrg";
                return _userId;
            }
     }*/
    class MockDataContext : IAppDataContext
    {
        
        public MockDataContext()
        {
            this.Users = new InMemoryDbSet<ApplicationUser>();
            Projects = new InMemoryDbSet<Project>();
            this.Files = new InMemoryDbSet<File>();
            this.Themes = new InMemoryDbSet<Theme>();
            UserProjects = new InMemoryDbSet<UserProject>();
            this.FileTypes = new InMemoryDbSet<FileTypes>();
        }
        public IDbSet<Project> Projects { get; set; }

        public IDbSet<File> Files { get; set; }

        public IDbSet<Theme> Themes { get; set; }

        public IDbSet<UserProject> UserProjects { get; set; }

        public IDbSet<FileTypes> FileTypes { get; set; }
   
        public InMemoryDbSet<ApplicationUser> Users { get; set; }

        public int SaveChanges()
        {
            int changes = 0;

            return changes;
        }

        public void Dispose()
        {
            // Do nothing!
        }
    }    
}
