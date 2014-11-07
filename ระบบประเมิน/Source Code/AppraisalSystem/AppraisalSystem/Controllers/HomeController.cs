﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppraisalSystem.Utility;

namespace AppraisalSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()//Login
        {

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [Permission]
        public ActionResult Manage()//Index (Choose search/Add new assets)
        {
            return View();
        }
    }
}
