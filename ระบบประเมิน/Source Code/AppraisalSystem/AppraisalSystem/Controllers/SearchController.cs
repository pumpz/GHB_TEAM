using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppraisalSystem.Models;
using AppraisalSystem.Utility;

namespace AppraisalSystem.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult ManageSearch()
        {
            List<AppraisalListsModel> searchList = null;

            return View(searchList);
        }

        [HttpPost]
        public ActionResult ManageSearch(FormCollection val)
        {
            List<AppraisalListsModel> searchList = searchResult(val);

            return View(searchList);
        }

        public List<AppraisalListsModel> searchResult(FormCollection val)
        {
            List<AppraisalListsModel> searchList = new List<AppraisalListsModel>();

            string appraisalCode = val["appraisalCode"];
            int provinceId = val["provinceId"] == null ? 38 : Convert.ToInt32(val["provinceId"]);
            int amphurId = val["amphurId"] == null ? 581 : Convert.ToInt32(val["amphurId"]);

            AppraisalService serv = new AppraisalService();
            searchList = serv.GetAppraisalLists(appraisalCode,provinceId,amphurId,"1",true);
            if (ContentHelpers.Isnull(searchList) || searchList.Count <= 0)
            {
                ModelState.AddModelError("", "Search data not found.");
            }  

            return searchList;
        }

        public ActionResult SearchResult()
        {
            return View();
        }
    }
}
