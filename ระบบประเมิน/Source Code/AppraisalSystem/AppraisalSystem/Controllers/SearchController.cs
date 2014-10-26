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

        public ActionResult ManageSearch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageSearch(FormCollection val)
        {
            return RedirectToAction("SearchResult", "Search");
        }

        public ActionResult SearchResult()
        {
            return View();
        }
    }
}
