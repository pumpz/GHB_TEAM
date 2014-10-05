using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppraisalSystem.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult SearchResult()
        {
            return View();
        }
    }
}
