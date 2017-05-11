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
using EditThor1.Handlers;
using EditThor1.Utilities;

namespace EditThor1.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectService service = new ProjectService();
        private FileService fileService = new FileService();
        private ThemeService themeService = new ThemeService();
        // GET: Project
        // returns create project site for logged in users
        public ActionResult CreateProject()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ProjectViewModel model = new ProjectViewModel();
            return View(model);
        }
        // creates project from information gathered by user for logged in users
        [HttpPost]
        public ActionResult CreateProject(ProjectViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            // checks if project with same name already exists created by same user
            if (service.checkSameName(model.name))
            {
                throw new Exception("Project already exists");
            }

            service.AddProject(model.name);

            return RedirectToAction("Index", "Home");
        }
        // opens editor on specific project with a specific file open, if no file is specified it opens first file in project
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult OpenEditor(int? id, string code, int? fileID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            // checks if user has access to project
            if (!service.HasAccess(Convert.ToInt32(id)))
            {
                //TODO: change to user doesn't have access
                throw new Exception("Project is Empty");
            }
            ListFileViewModel model = new ListFileViewModel();
            ThemeViewModel themeModel = new ThemeViewModel();

            model.AllFiles = service.OpenProject(id);

            if(model.AllFiles.Count == 0)
            {
                return RedirectToAction("CreateFile", new { id = id });
            }
            if (fileID == null)
            {
                // redirects to get first file to display
                return RedirectToAction("DisplayFile", new {id = model.AllFiles.First().ID , projectID = id});
            }

            ViewBag.ProjectName = service.GetProjectName(Convert.ToInt32(id));
            model.projectId = Convert.ToInt32(id);
            model.Content = code;
            model.projectId = Convert.ToInt32(id);
            model.fileId = Convert.ToInt32(fileID);
            model.filetype = fileService.GetFileTypeName(Convert.ToInt32(fileID));
            model.theme = themeService.CallTheme();
            model.Users = service.UserListofSharedProject(Convert.ToInt32(id));
            return View(model);
        }
        // saves file to database and redirects to open editor on same file
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(ListFileViewModel model)
        {
            if (model.Content != null)
            {
                byte[] array = Encoding.ASCII.GetBytes(model.Content);
                fileService.SaveFile(array, model.fileId);
            }
            else
            {
                byte[] array = new byte[0];
                fileService.SaveFile(array, model.fileId);
            }
            
            return RedirectToAction("OpenEditor", "Project", new { id = model.projectId, code = model.Content, fileID = model.fileId });
        }
        // gets file from database and redirects to open editor with projectid, content(code) of file and file id
        [HttpGet]
        public ActionResult DisplayFile(int? id, int? projectID)
        {
            ViewBag.code = Encoding.Default.GetString(fileService.GetFiles(id, projectID));

            return RedirectToAction("OpenEditor", "Project", new { id = projectID, code = ViewBag.code, fileID = id });
        }
        // deletes project if user is owner of project
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
                // þurfum ad gera aðeins betri villumeðhöndlun
                throw new DeleteException();
            }
            // þurfum ad gera aðeins betri villumeðhöndlun
            throw new DeleteException();
        }
        // returns share project window where you can add user to project by email
        [HttpGet]
        public ActionResult ShareProject(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            
            UserViewModel model = new UserViewModel();
            model.ProjectID = Convert.ToInt32(id);
            return View(model);
        }
        // takes in view model with project id and email of other user, checks if he exists then gets his id and sends to function in product
        [HttpPost]
        public ActionResult ShareProject(UserViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!service.IsRegisteredUser(model.userName))
            {
                throw new NotRegisteredException("User isn't registered.");
            }

            model.ID = service.GetUserID(model.userName);

            if (service.UserHasAccess(model.ID, model.ProjectID))
            {
                throw new Exception("User already has access to this project");
            }

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
            model.type = fileService.GetAvailableTypes();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateFile(FileViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            fileService.CreateFile(model.projectID, model.name, model.typeID);
            return RedirectToAction("OpenEditor", "Project", new { id = model.projectID });
        }

        [HttpPost]
        public ActionResult DeleteFile(FormCollection model)
        {

            ListFileViewModel data = new ListFileViewModel();
            UpdateModel(data);

            fileService.DeleteFile(data.fileId);

            return RedirectToAction("OpenEditor", "Project", new { id = data.projectId });
        }

        [HttpGet]
        public ActionResult LeaveProject(int? projectID)
        {
            if(projectID == null)
            {
                //todo
            }
            service.LeaveProject(Convert.ToInt32(projectID));
            return RedirectToAction("Index", "Home");
        }


    }
}