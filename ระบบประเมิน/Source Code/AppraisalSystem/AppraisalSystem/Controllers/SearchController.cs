using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppraisalSystem.Models;
using AppraisalSystem.Utility;
using System.Web.Routing;

namespace AppraisalSystem.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        public IConditionService ConditionService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (ConditionService == null) { ConditionService = new ConditionService(); }

            base.Initialize(requestContext);
        }

        [Permission]
        public ActionResult ManageSearch()
        {
            // Clear Data that set for Layout menu
            ContentHelpers.clearParamForSetMenu();

            List<AppraisalListsModel> searchList = null;

            setSearchFilter();

            return View(searchList);
        }

        [HttpPost]
        [Permission]
        public ActionResult ManageSearch(FormCollection val)
        {
            List<AppraisalListsModel> searchList = searchResult(val);

            setSearchFilter();

            return View(searchList);
        }

        public List<AppraisalListsModel> searchResult(FormCollection val)
        {
            List<AppraisalListsModel> searchList = new List<AppraisalListsModel>();

            string appraisalCode = val["appraisalCode"];
            int districtId = val["districtId"] == null ? 38 : Convert.ToInt32(val["districtId"]);
            int amphurId = val["amphurId"] == null ? 581 : Convert.ToInt32(val["amphurId"]);
            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));

            AppraisalService serv = new AppraisalService();
            searchList = serv.GetAppraisalLists(appraisalCode, districtId, amphurId, userName, true);
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

        public void setSearchFilter()
        {
            ViewData["AMPHUR"] = ConditionService.GetAmphurLists();
            ViewData["DISTRICT"] = ConditionService.GetDistrictLists();
        }



    }
}
