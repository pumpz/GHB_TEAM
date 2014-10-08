using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppraisalSystem.Controllers
{
    public class BackendController : Controller
    {
        //
        // GET: /Backend/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult ManagePermissions()
        {
            return View();
        }
    }
}
