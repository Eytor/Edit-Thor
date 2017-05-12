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
        // Returns the Create project site for logged in users.
        public ActionResult CreateProject()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ProjectViewModel model = new ProjectViewModel();
            return View(model);
        }
        // Creates a project from information gathered by the user for logged in users.
        [HttpPost]
        public ActionResult CreateProject(ProjectViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            // Checks if a project with same name already exists for the same user.
            if (service.checkSameName(model.Name))
            {
                throw new Exception("Project already exists");
            }

            service.AddProject(model.Name);

            return RedirectToAction("Index", "Home");
        }
        // Opens the editor on a specific project with a specific file open. If no file is specified it opens the first file in the project.
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult OpenEditor(int? id, string code, int? fileID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            // Checks if the user has access to the project.
            if (!service.HasAccess(Convert.ToInt32(id)))
            {
                throw new Exception("Project is Empty or no access");
            }
            ListFileViewModel model = new ListFileViewModel();
            ThemeViewModel themeModel = new ThemeViewModel();

            model.AllFiles = service.GetAllFiles(Convert.ToInt32(id));

            if(model.AllFiles.Count == 0)
            {
                return RedirectToAction("CreateFile", new { id = id });
            }
            if (fileID == null)
            {
                // Redirects to get first file to display.
                return RedirectToAction("DisplayFile", new {id = model.AllFiles.First().ID , projectID = id});
            }

            ViewBag.ProjectName = service.GetProjectName(Convert.ToInt32(id));
            model.ProjectId = Convert.ToInt32(id);
            model.Content = code;
            model.ProjectId = Convert.ToInt32(id);
            model.FileId = Convert.ToInt32(fileID);
            model.Filetype = fileService.GetFileTypeName(Convert.ToInt32(fileID));
            model.Theme = themeService.CallTheme();
            model.Users = service.UserListofSharedProject(Convert.ToInt32(id));
            return View(model);
        }
        // Saves file to database and redirects to OpenEditor on the same file.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(ListFileViewModel model)
        {
            if (model.Content != null)
            {
                byte[] array = Encoding.ASCII.GetBytes(model.Content);
                fileService.SaveFile(array, model.FileId);
            }
            else
            {
                byte[] array = new byte[0];
                fileService.SaveFile(array, model.FileId);
            }
            
            return RedirectToAction("OpenEditor", "Project", new { id = model.ProjectId, code = model.Content, fileID = model.FileId });
        }
        // Gets file from database and redirects to OpenEditor with the ProjectID, content(code) of file and file id.
        [HttpGet]
        public ActionResult DisplayFile(int? id, int? projectID)
        {
            ViewBag.code = Encoding.Default.GetString(fileService.GetFiles(id, projectID));

            return RedirectToAction("OpenEditor", "Project", new { id = projectID, code = ViewBag.code, fileID = id });
        }
        // Deletes the project if user is owner of project.
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
        // Returns share project window where you can add user to project by email.
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
        // Takes in view model with project id and email of other user, checks if he exists then gets his id and sends to function in product.
        [HttpPost]
        public ActionResult ShareProject(UserViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!service.IsRegisteredUser(model.UserName))
            {
                throw new NotRegisteredException("User isn't registered.");
            }

            model.ID = service.GetUserID(model.UserName);

            if (service.UserHasAccess(model.ID, model.ProjectID))
            {
                throw new Exception("User already has access to this project");
            }

            service.ShareProject(model.ID, model.ProjectID);
            return RedirectToAction("Index", "Home");
        }
        // Downloads all files from project as (project name).zip.
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
                        var zipEntry = zipArchive.CreateEntry(file.Name);

                        //Sækir strauminn
                        using (var originalFileStream = new MemoryStream(file.TheFile))
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
        // Returns view for user to create file with name and file type.
        [HttpGet]
        public ActionResult CreateFile(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.ProjectID = id;
            FileViewModel model = new FileViewModel();
            model.ProjectID = Convert.ToInt32(id);
            model.Type = fileService.GetAvailableTypes();
            return View(model);
        }
        // Takes input from user and creates file sends info to fileservice to be created.
        [HttpPost]
        public ActionResult CreateFile(FileViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            fileService.CreateFile(model.ProjectID, model.Name, model.TypeID);
            return RedirectToAction("OpenEditor", "Project", new { id = model.ProjectID });
        }
        // Takes model carrying file id and sends to file service for deletion then redirects to open same project again.
        [HttpPost]
        public ActionResult DeleteFile(ListFileViewModel model)
        {
            fileService.DeleteFile(model.FileId);

            return RedirectToAction("OpenEditor", "Project", new { id = model.ProjectId });
        }
        // Checks if user has access to project and if project exist then removes current user from project.
        [HttpGet]
        public ActionResult LeaveProject(int? projectID)
        {
            if (projectID == null)
            {
                throw new LeaveProjectException();
            }
            if (!service.ProjectExists(Convert.ToInt32(projectID)))
            {
                throw new LeaveProjectException();
            }
            service.LeaveProject(Convert.ToInt32(projectID));
            return RedirectToAction("Index", "Home");
        }


    }
}