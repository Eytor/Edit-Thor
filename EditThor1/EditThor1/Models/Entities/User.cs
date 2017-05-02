using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditThor1.Models.Entities
{
    public class User
    {
        //ID of the user
        public int ID { get; set; }

        //the name of the user
        public string name { get; set; }

        //the password of the user
        public string password { get; set; }

        //the email of the user
        public string email { get; set; }

        //foreign key of the theme
        public int themeID { get; set; }
    }
}