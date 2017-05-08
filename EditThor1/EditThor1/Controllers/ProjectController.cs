using EditThor1.Models;
using EditThor1.Models.ViewModels;
using EditThor1.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

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
          
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(FormCollection model)
        {

            ListFileViewModel data = new ListFileViewModel();
            UpdateModel(data);

            

            if (data.Content == null)
            {
                //todo error ?
            }


            byte[] array = Encoding.ASCII.GetBytes(data.Content);
            fileService.SaveFile(array, 2);

            /* byte[] content;
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
             fileService.SaveFile(content, id);*/



            return View("OpenEditor");
        }

        [HttpGet]
        public ActionResult DisplayFile(int? fileId, int? ProjectId)
        {
            ViewBag.str = Encoding.Default.GetString(fileService.GetFiles(2, ProjectId));

            return View("OpenEditor/"+ ProjectId);
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

        [HttpGet]
        public FileResult Download(int id)
        {
            // af því "File" er frátekið í Systems.IO þarf að skrifa út allt namespacið
            List<EditThor1.Models.Entities.File> files = service.GetAllFiles(id);

            using (var compressedFileStream = new MemoryStream())
            {
                //Býr til möppu og vistar strauminn í archive
                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                {
                    foreach (var file in files)
                    {
                        //Býr til zipEntry fyrir hverja skrá
                        var zipEntry = zipArchive.CreateEntry(file.name);

                        //Sækir strauminn
                        using (var originalFileStream = new MemoryStream(file.file))
                        {
                            using (var zipEntryStream = zipEntry.Open())
                            {
                                //Afritar strauminn í zip entry strauminn
                                originalFileStream.CopyTo(zipEntryStream);
                            }
                        }
                    }
                }

                return new FileContentResult(compressedFileStream.ToArray(), "application/zip") { FileDownloadName = "archive.zip" };
            }

        }

    }
}