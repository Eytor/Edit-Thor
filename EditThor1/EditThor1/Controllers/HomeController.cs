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

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Editor()
        {
            return RedirectToAction("OpenEditor", "Project");
        }

        public ActionResult Project()
        {
            //makar þetta eitthvað sens?
            return RedirectToAction("CreateProject" , "Project");
        }

        [HttpGet]
        public ActionResult OpenEditor(int? id)
        {
            FileViewModel model = new FileViewModel();

            if (id == null)
            {
                throw new HttpException(404, "Project is Empty");
            }
            return View(service.OpenProject(id));
        }

    }
}