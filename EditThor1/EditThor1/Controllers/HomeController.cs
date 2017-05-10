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

        public ActionResult Index(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ListProjectViewModel listAll = new ListProjectViewModel();
            if (id == null)
            {
                ViewBag.Title = "All Projects";
                listAll.AllProject = service.GetAllUserProjects();

            }
            else if(id == 1)
            {
                ViewBag.Title = "My Projects";
                listAll.AllProject = service.GetMyProjects();
            }
            else
            {
                ViewBag.Title = "Shared with me";
                listAll.AllProject = service.GetSharedProjects();
            }
            
            

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

        public ActionResult Project()
        {
            //makar þetta eitthvað sens? nei, hvad atti thetta ad gera?
            return RedirectToAction("CreateProject" , "Project");
        }

        [HttpGet]
        public ActionResult OpenEditor(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            FileViewModel model = new FileViewModel();

            if (id == null)
            {
                throw new HttpException(404, "Project is Empty");
            }
            return View(service.OpenProject(id));
        }

    }
}