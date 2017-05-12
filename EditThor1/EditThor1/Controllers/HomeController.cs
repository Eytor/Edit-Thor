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
        
        // Returns the main site where all projects are displayed. Checks the user identity to know which projects to display for logged in users.
        public ActionResult Index(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            ListProjectViewModel listAll = new ListProjectViewModel();
            // If there is no id it will display all projects related to user.
            if (id == null)
            {
                ViewBag.Title = "All Projects";
                listAll.AllProject = service.GetAllUserProjects();
            }
            // If id is 1 it will only display projects that current user created.
            else if(id == 1)
            {
                ViewBag.Title = "My Projects";
                listAll.AllProject = service.GetMyProjects();
            }
            // Otherwise it will display projects that have been shared with the current user.
            else
            {
                ViewBag.Title = "Shared with me";
                listAll.AllProject = service.GetSharedProjects();
            }
            
            return View(listAll);
        }
        // Returns the Help site for logged in users.
        public ActionResult Help()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        
        // Returns the site where users can select different themes for the Ace editor for logged in users.
        [HttpGet]
        public ActionResult Themes()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ThemeViewModel model = new ThemeViewModel();
            model.ThemeList = themeService.GetThemes();
      
            return View(model);
        }
        // Calls a function in the Theme service where it sets the selected theme for logged in users.
        [HttpPost]
        public ActionResult Themes(ThemeViewModel model)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            themeService.SetTheme(model.ThemeID);
            return RedirectToAction("Index", "Home");
           
        }
       

    }
}