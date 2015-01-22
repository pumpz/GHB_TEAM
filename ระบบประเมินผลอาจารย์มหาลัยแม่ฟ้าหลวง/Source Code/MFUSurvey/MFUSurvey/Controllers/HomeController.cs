using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MFUSurvey.Models;

namespace MFUSurvey.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = Resources.System_Config.TITLE_HOME_EN;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
