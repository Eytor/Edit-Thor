﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EditThor1.Models.Entities
{
    public class UserProject
    {
        public int ID { get; set; }

        public int ProjectID { get; set; }

        public string UserID { get; set; }
    }
}