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
        private FileService fileService = new FileService();
        // GET: Project
        

        public ActionResult CreateProject()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ListFileViewModel model = new ListFileViewModel();
            model.AllFiles = service.OpenProject(id);
            var str = System.Text.Encoding.Default.GetString(fileService.GetFiles(id));

            /* for (int i = 0; i < str.Length; i++)
             {
                 ViewBag.displayText += str[i];
             }*/
            for (int i = 0; i < 10; i++)
            {
                ViewBag.displayText = "veii stebbi er bestur";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(ListFileViewModel model, int id)
        {
          
            byte[] content;
            if (Request.Files != null && Request.Files.Count == 1)
            {
                
                var file = Request.Files[0];
               
                if (file != null && file.ContentLength > 0)
                {
                    content = new byte[file.ContentLength];
                    file.InputStream.Read(content, 0, file.ContentLength);            
                }
            }
            content = model.Content.Select(byte.Parse).ToArray();
            fileService.SaveFile(content, id);
            return View("OpenProject");
        }

        [HttpGet]
        public ActionResult DisplayFile(int id)
        {
            var str = System.Text.Encoding.Default.GetString(fileService.GetFiles(id));

            for (int i = 0; i < str.Length; i++)
            {
                ViewBag.displayText += str[i];
            }

            return View("OpenProject");
        }

        [HttpGet]
        public ActionResult DeleteProject(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id != null)
            {
                if(service.ProjectExists(id))
                {
                    service.DeleteProject(id);
                    return RedirectToAction("Index", "Home");
                }
                return HttpNotFound();
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult ShareProject(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.ProjectID = id;
            UserViewModel model = new UserViewModel();
            model.ProjectID = Convert.ToInt32(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult ShareProject(UserViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            string userName = model.userName;
            model.ID = service.GetUserID(userName);
            service.ShareProject(model.ID, model.ProjectID);
            return RedirectToAction("Index", "Home");
        }

    }
}