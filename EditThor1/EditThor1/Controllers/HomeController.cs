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
        private ThemeService themeService = new ThemeService();
        
        // returns main site where all projects are displayed takes if wich is an identifyer for what projects to display for logged in users
        public ActionResult Index(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            ListProjectViewModel listAll = new ListProjectViewModel();
            //if there is no id it will displat all projects related to user
            if (id == null)
            {
                ViewBag.Title = "All Projects";
                listAll.AllProject = service.GetAllUserProjects();
            }
            //if id is 1 it will only display projects that current user created
            else if(id == 1)
            {
                ViewBag.Title = "My Projects";
                listAll.AllProject = service.GetMyProjects();
            }
            // otherwise it will display projects that have been shared with current user
            else
            {
                ViewBag.Title = "Shared with me";
                listAll.AllProject = service.GetSharedProjects();
            }
            
            return View(listAll);
        }
        // returns help site for logged in users
        public ActionResult Help()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        
        // returns site where users can select themes for ace editior for logged in users
        [HttpGet]
        public ActionResult Themes()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ThemeViewModel model = new ThemeViewModel();
            model.themeList = themeService.GetThemes();
      
            return View(model);
        }
        // calls function in theme service where it sets theme as the one you just picked for logged in users
        [HttpPost]
        public ActionResult Themes(ThemeViewModel model)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            themeService.SetTheme(model.themeID);
            return RedirectToAction("Index", "Home");
           
        }
       

    }
}