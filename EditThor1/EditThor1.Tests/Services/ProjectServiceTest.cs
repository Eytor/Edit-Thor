using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using EditThor1.Services;
using EditThor1.Models.Entities;
using System.Web;

namespace EditThor1.Tests.Services
{
    [TestClass]
    public class ProjectSerivceTest
    {
        private ProjectService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();

            var t1 = new Project
            {
                ID = 1,
                Name = "Pro1",
                OwnerID = "2seg",
                OwnerName = "Dabs",
                FilesList = null,
                Userslist = null,
             };
            mockDb.Projects.Add(t1);
            
            
            _service = new ProjectService(mockDb);
        
         }

        [TestMethod]
        public void TestAddProject()
        {
            // Arrange:
            Project newProject = new Project();
            newProject.Name = "testPro";

            // Act:
            _service.AddProject("testPro");

            // Assert:
            Assert.AreNotEqual(0, _service.GetAllUserProjects().Count);
            Assert.AreEqual(1, _service.GetAllUserProjects().Count);
            Assert.AreNotEqual(2, _service.GetAllUserProjects().Count);

        }

        [TestMethod]
        public void TestGetAllProjects()
        {
            // Arrange:
            Project newProject = new Project();

            // Act:
            var result = _service.GetAllUserProjects();

            // Assert:
            Assert.AreEqual(1, _service.GetAllUserProjects().Count);

            Assert.AreNotEqual(0, _service.GetAllUserProjects().Count);

            Assert.AreNotEqual(2, _service.GetAllUserProjects().Count);
        }
    }
}
