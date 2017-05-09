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
        public ActionResult OpenEditor(int? id, string code, int? fileID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.fileID = fileID;
            ListFileViewModel model = new ListFileViewModel();
            model.AllFiles = service.OpenProject(id);
            if(fileID != null)
            {
                model.projectId = Convert.ToInt32(id);
                model.fileId = Convert.ToInt32(fileID);
            }
            ViewBag.code = code;
            ViewBag.DocumentId = id;
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
            fileService.SaveFile(array, data.fileId );

            return RedirectToAction("OpenEditor", "Project", new { id = data.projectId });
        }

        [HttpGet]
        public ActionResult DisplayFile(int? id, int? projectID)
        {
            ViewBag.code = Encoding.Default.GetString(fileService.GetFiles(id, projectID));

            return RedirectToAction("OpenEditor", "Project", new { id = projectID, code = ViewBag.code, fileID = id });
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
                if (service.ProjectExists(id))
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
            string projectName = "";
            if (service.GetProjectName(id) != null)
            {
                projectName = service.GetProjectName(id) + ".zip";
            }

            // af því "File" er frátekið í Systems.IO þarf að skrifa út allt namespacið
            List<Models.Entities.File> files = service.GetAllFiles(id);

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

                return new FileContentResult(compressedFileStream.ToArray(), "application/zip") { FileDownloadName = projectName };
            }
        }

        [HttpGet]
        public ActionResult CreateFile(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.ProjectID = id;
            FileViewModel model = new FileViewModel();
            model.projectID = Convert.ToInt32(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateFile(FileViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            fileService.CreateFile(model.projectID, model.name, model.type);
            return RedirectToAction("OpenEditor", "Project", new { id = model.projectID });
        }
    }
}