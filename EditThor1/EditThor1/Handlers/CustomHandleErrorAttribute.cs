﻿using EditThor1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EditThor1.Handlers
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    { 
        //yfirskrifum OnException aðgerðina
        public override void OnException(ExceptionContext filterContext)
        {
            // Get exception
            Exception ex = filterContext.Exception;
            Logger.Instance.LogException(ex);

            //Set the view name to be returned, different error views for different exception types
            string viewName = "";

            //Get current controller and action
            string currentController = (string)filterContext.RouteData.Values["controller"];
            string currentActionName = (string)filterContext.RouteData.Values["action"];

            if (ex is Exception)
            {
                if(currentController == "Project" && currentActionName == "ShareProject")
                { 
                    viewName = "ErrorShareProject";
                }
                else if (currentController == "Project" && currentActionName == "CreateProject")
                {
                    viewName = "ErrorCreateProject";
                }
                else
                {
                    viewName = "Error";
                }
            }
            HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, currentController, currentActionName);
            ViewResult result = new ViewResult
            {
                ViewName = viewName,
                TempData = filterContext.Controller.TempData
            };

            filterContext.Result = result;
            filterContext.ExceptionHandled = true;

            // Call the base class implementation:
            base.OnException(filterContext);
        }
    }
}