﻿using EditThor1.Models;
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

        [HttpPost]
        public ActionResult Save(string[] arr)
        {
            if (Request.Files != null && Request.Files.Count == 1)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var content = new byte[file.ContentLength];
                    file.InputStream.Read(content, 0, file.ContentLength);
                    content = arr.Select(byte.Parse).ToArray();
                }
            }
            return RedirectToAction("Editor", "Project");
        }

    }
}