using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EditThor1.Models;
using EditThor1.Models.Entities;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;


namespace EditThor1.Services
{
    public class ThemeService
    {
        private readonly IAppDataContext db;

        public ThemeService(IAppDataContext dbContext)
        {
            db = dbContext ?? new ApplicationDbContext();
        }

        public ThemeService()
        {
            _db = new ApplicationDbContext();
        }

        private string _userId = HttpContext.Current.User.Identity.GetUserId();
        private ApplicationDbContext _db = new ApplicationDbContext();


        // Function creates a list of all themes and returns it for a dropdown list.
        public List<SelectListItem> GetThemes()
        {
            List<SelectListItem> sendThemes = new List<SelectListItem>();

            sendThemes.Add(new SelectListItem() { Value = "", Text = " - Choose Theme - " });

            _db.Themes.ToList().ForEach((x) =>
            {
                sendThemes.Add(new SelectListItem() { Value = x.ID.ToString(), Text = x.Name });
            });

            return sendThemes;
        }
        // Function sets current users theme by theme id.
        public void SetTheme(int themeID)
        {
            ApplicationUser model = (from t in _db.Users
                                     where t.Id == _userId
                                     select t).SingleOrDefault();
            model.themeId = themeID;

            _db.SaveChanges();
        }
        // Funtion gets the name of theme set to current user and returns it in lowercase.
        public string CallTheme()
        {
            int userThemeID = (from t in _db.Users
                               where t.Id == _userId
                               select t.themeId).SingleOrDefault();

            string currentTheme = (from t in _db.Themes
                                   where t.ID == userThemeID
                                   select t.Name).SingleOrDefault();

            return currentTheme.ToLower();
        }
    }
}