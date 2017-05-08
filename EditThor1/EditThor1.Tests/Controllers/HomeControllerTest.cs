using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EditThor1;
using EditThor1.Controllers;

namespace EditThor1.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
<<<<<<< HEAD
        //    ViewResult result = controller.About() as ViewResult;

            // Assert
      //      Assert.AreEqual("Your application description page.", result.ViewBag.Message);
=======
            //ViewResult result = controller.About() as ViewResult;

            // Assert
            //Assert.AreEqual("Your application description page.", result.ViewBag.Message);
>>>>>>> 1e52eb7cd4d27a983149bc830e61b21fa8b8678c
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
