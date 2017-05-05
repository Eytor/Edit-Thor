using EditThor1.Models;
using EditThor1.Models.ViewModels;
using EditThor1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EditThor1.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectService service = new ProjectService();
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateProject()
        {
            ProjectViewModel model = new ProjectViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateProject(FormCollection formData)
        {
            ProjectViewModel model = new ProjectViewModel();
            UpdateModel(model);

            var names = model.name;

            service.AddProject(names);

            return RedirectToAction("Index", "Home");
        }
    }
}