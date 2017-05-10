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
        private string _userId = HttpContext.Current.User.Identity.GetUserId();
        private ApplicationDbContext _db = new ApplicationDbContext();



        public List<SelectListItem> GetThemes()
        {
            List<SelectListItem> sendThemes = new List<SelectListItem>();

            sendThemes.Add(new SelectListItem() { Value = "", Text = " - Choose Theme - " });

            _db.Themes.ToList().ForEach((x) =>
            {
                sendThemes.Add(new SelectListItem() { Value = x.ID.ToString(), Text = x.name });
            });

            return sendThemes;
        }

        public void SetTheme(int id)
        {
   
            ApplicationUser model = (from t in _db.Users
                                     where t.Id == _userId
                                     select t).SingleOrDefault();
            model.themeId = id;

            _db.SaveChanges();
        }

        public string CallTheme()
        {
            
            int userIdTheme = (from t in _db.Users
                               where t.Id == _userId
                               select t.themeId).SingleOrDefault();

            string currentTheme = (from t in _db.Themes
                                   where t.ID == userIdTheme
                                   select t.name).SingleOrDefault();

            return currentTheme.ToLower();
        }
    }
}