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

            setAmphur();
            setDistrict();

            return View(searchList);
        }

        [HttpPost]
        public ActionResult ManageSearch(FormCollection val)
        {
            List<AppraisalListsModel> searchList = searchResult(val);

            setAmphur();
            setDistrict();

            return View(searchList);
        }

        public List<AppraisalListsModel> searchResult( FormCollection val)
        {
            List<AppraisalListsModel> searchList = new List<AppraisalListsModel>();

            string appraisalCode = val["appraisalCode"];
            int districtId = val["districtId"] == null ? 38 : Convert.ToInt32(val["districtId"]);
            int amphurId = val["amphurId"] == null ? 581 : Convert.ToInt32(val["amphurId"]);

            AppraisalService serv = new AppraisalService();
            searchList = serv.GetAppraisalLists(appraisalCode, districtId, amphurId, "1", true);
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

        public void setAmphur()
        {
            ConditionService model = new ConditionService();

            AmphurModel item = new AmphurModel();
            item.amphur_id = -1;
            item.amphur_name = "โปรดเลือก";

            List<AmphurModel> modelList = model.GetAmphurLists();
            modelList.Insert(0, item);

            ViewData["Amphur"] = modelList;
        }

        public void setDistrict()
        {
            ConditionService model = new ConditionService();

            DistrictModel item = new DistrictModel();
            item.district_id = -1;
            item.district_name = "โปรดเลือก";

            List<DistrictModel> modelList = model.GetDistrictLists();
            modelList.Insert(0, item);

            ViewData["District"] = modelList;
        }

    }
}
