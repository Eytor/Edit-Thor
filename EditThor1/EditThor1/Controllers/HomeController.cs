using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EditThor1.Services;
using EditThor1.Models.ViewModels;

namespace EditThor1.Controllers
{
    public class HomeController : Controller
    {
        private ProjectService service = new ProjectService();

        public ActionResult Index()
        {

            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ListProjectViewModel listAll = new ListProjectViewModel();
            listAll.AllProject = service.GetAllUserProjects();

            return View(listAll);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Editor()
        {
            return View();
        }

        public ActionResult Project()
        {
            //makar þetta eitthvað sens?
            return RedirectToAction("CreateProject" , "Project");
        }

    }
}