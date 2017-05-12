using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using EditThor1.Services;
using EditThor1.Models.Entities;
using System.Web;
using EditThor1.Models;
using EditThor1.Tests.Services;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EditThor1.Tests.Services
{
    [TestClass]
    public class ProjectSerivceTest
    {
        private const string userID1 = "1772c5bf-c364-4fd7-9804-8d821b1ff59a";
        private const string userID2 = "1facd3ed-d7b5-4301-aa82-f482def6b639";
        private const string userID3 = "202cba1a-e274-4fe3-a015-61236b14ce56";
        private const string userID4 = "525ab4f4-1f5a-4dad-a865-fa0227e9fca2";
        private const string userID5 = "6c997b4e-3061-4060-9352-f678c8349739";

        private const string userEm1 = "dabs@ru.is";
        private const string userEm2 = "svanhvit@ru.is";
        private const string userEm3 = "stebbi@ru.is";
        private const string userEm4 = "eythor@ru.is";
        private const string userEm5 = "sara@ru.is";

        private ProjectService _service;
        

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();

            var user = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = userID2,
                    Email = userEm2,
                    UserName = userEm2,                    
                },
                new ApplicationUser
                {
                    Id = userID3,
                    Email = userEm3,
                    UserName = userEm3,
                },
            };


            var pro1 = new Project()
            {
                ID = 1,
                Name = "pro1",
                OwnerID = userID1,
                OwnerName = userEm1,
                
            };
            mockDb.Projects.Add(pro1);

            var pro2 = new Project()
            {
                ID = 2,
                Name = "pro2",
                OwnerID = userID4,
                OwnerName = userEm4,

            };
            mockDb.Projects.Add(pro2);

            
            
            _service = new ProjectService(mockDb);
        
         }

        [TestMethod]
        public void TestAddProject()
        {
            // Arrange:
            Project newProject = new Project();

            newProject.ID = 1;
            newProject.Name = "testPro";
            newProject.OwnerID = userID2;
            newProject.OwnerName = userEm2;

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
            Assert.IsNotNull(result);
            Assert.AreEqual(1, _service.GetAllUserProjects().Count);
            Assert.AreNotEqual(0, _service.GetAllUserProjects().Count);
            Assert.AreNotEqual(2, _service.GetAllUserProjects().Count);
        }

        [TestMethod]
        public void TestGetAllSharedProjects()
        {
            // Arrange:
            Project newProject = new Project();
            newProject.ID = 1;
            newProject.Name = "testPro";
            newProject.OwnerID = userID2;
            newProject.OwnerName = userEm2;
            newProject.Userslist = new List<ApplicationUser>();

            // Act:
            var result = _service.GetSharedProjects();

            // Assert:
            Assert.AreEqual(1, _service.GetSharedProjects().Count);
            Assert.AreNotEqual(0, _service.GetSharedProjects().Count);
            Assert.AreNotEqual(2, _service.GetSharedProjects().Count);
        }

        [TestMethod]
        public void TestProjectById()
        {
            Project newProject = new Project();
            newProject.ID = 1;

            var result = _service.GetProjectById(1);

            Assert.AreEqual(1, result);
        }
        
        [TestMethod]
        public void TestGetUserId()
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = userEm2;
            user.Email = userEm2;
            user.Id = userID2;

            var result = _service.GetUserID(userEm2);

            Assert.AreEqual(userID2, result);
        }

        [TestMethod]
        public void TestisOwner()
        {
            Project pro = new Project();
            pro.OwnerName = userEm4;
            pro.OwnerID = userID4;
            
        }

    }
}
