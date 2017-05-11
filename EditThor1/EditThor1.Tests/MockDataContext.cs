using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EditThor1.Models;
using System.Data.Entity;
using EditThor1.Models.Entities;
using System.Data.Entity.Infrastructure;

namespace EditThor1.Tests
{
    /// <summary>
    /// Summary description for MockDataContext
    /// </summary>
    class MockDataContext : IAppDataContext
    {
        public MockDataContext()
        {
            Projects = new InMemoryDbSet<Project>();
            Files = new InMemoryDbSet<File>();
            Themes = new InMemoryDbSet<Theme>();
            UserProjects = new InMemoryDbSet<UserProject>();
            FileTypes = new InMemoryDbSet<FileTypes>();
        }
        public IDbSet<Project> Projects { get; set; }

        public IDbSet<File> Files { get; set; }

        public IDbSet<Theme> Themes { get; set; }

        public IDbSet<UserProject> UserProjects { get; set; }

        public IDbSet<FileTypes> FileTypes { get; set; }


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
