using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EditThor1.Models.Entities;
using EditThor1.Services;

namespace EditThor1.Tests.Services
{

    [TestClass]
    public class UnitTest2
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

        private FileService _service;
        
        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();

            var project = new Project
            {
                ID = 1,
                Name = "pro1",
                OwnerID = userID1,
                OwnerName = userEm1,
                FilesList = null,
                Userslist = null,
            };

            var file = new File
            {
                Name = "index.html",
                TypeID = 1,
                TheFile = new byte[64],
                ProjectID = 1,
            };
            mockDb.Files.Add(file);

            var file1 = new File
            {
                Name = "index.html",
                TypeID = 2,
                TheFile = new byte[64],
                ProjectID = 2,
            };
            mockDb.Files.Add(file1);

            _service = new FileService(mockDb);
        }

        [TestMethod]
        public void TestSaveFile()
        {
            Project add = new Project();
            File file = new File();
            add.ID = 1;
            add.Name = "Pro1";
            add.OwnerID = "2a";
            add.OwnerName = "birna@ru.is";
            file.Name = "index.html";
            file.TypeID = 1;
            file.TheFile = new byte[64];
            file.ProjectID = 1;

            _service.SaveFile(null, 1);

            Assert.AreEqual(0, _service.Equals(0));
        }

        [TestMethod]
        public void TestGetFiles()
        {
            // Arrange:
            Project add = new Project();
            File file = new File();
            add.ID = 1;
            add.Name = "pro1";
            add.OwnerID = userID1;
            add.OwnerName = userEm1;
            file.Name = "index.html";
            file.TypeID = 1;
            file.TheFile = new byte[64];
            file.ProjectID = 1;

            // Act:
            var result = _service.GetFiles(1, 1);

            // Assert:
            Assert.AreEqual(file, result);
        }

         [TestMethod]
         public void TestCreateFile()
        {
            // Arrange:
            File file = new File();
            Project pro = new Project();
            pro.ID = 1;
            pro.Name = "pro1";
            pro.OwnerID = userID5;
            pro.OwnerName = userEm5;
            file.Name = "index.html";
            file.TypeID = 1;
            file.TheFile = new byte[64];
            file.ProjectID = 1;


            // Act:
            _service.CreateFile(1, "index.html", 1);

            var result = _service.GetFiles(1, 1);


            // Assert:
            Assert.AreEqual(result, file);           
        }
    }
}
