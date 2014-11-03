using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppraisalSystem.Models;
using System.Collections;
using System.Web.Routing;
using System.IO;
using AppraisalSystem.Utility;

namespace AppraisalSystem.Controllers
{
    public class ManageController : Controller
    {

        public IAppraisalService AppraisalService { get; set; }
        public IConditionService ConditionService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (AppraisalService == null) { AppraisalService = new AppraisalService(); }

            if (ConditionService == null) { ConditionService = new ConditionService(); }

            base.Initialize(requestContext);
        }
        //
        // GET: /Manage/

        public ActionResult ManageAssetDetail()//ข้อมูลที่ตั้งทรัพย์สิน
        {
            return View();
        }

        public ActionResult ManageAssetMap()//แผนที่
        {
            return View(new MapAssetModel());
        }
        
        [HttpPost]
        public ActionResult ManageAssetMap(MapAssetModel model)//แผนที่
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                    string userName = "system";
                    // Attempt to register the user
                    model.appraisal_assets_id = 1;
                    Boolean process = AppraisalService.MngMapAsset(model, userName);
                    if (Convert.ToBoolean(process))
                    {
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("", Convert.ToString("Insert map not success."));
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View();
        }

        [Permission]
        public ActionResult ManageAssetDoc(string id)//เอกสารสิทธิ์
        {
            AppraisalDetailModel model = new AppraisalDetailModel();
            try
            {
                ViewData["TYPE_OF_DOCUMENT"] = ConditionService.GetFilterLists("TYPE_OF_DOCUMENT");
                ViewData["CONDITION_LAND"] = ConditionService.GetFilterLists("CONDITION_LAND");
                ViewData["PROVINCE"] = ConditionService.GetProvinceLists();
                ViewData["AMPHUR"] = ConditionService.GetAmphurLists();
                ViewData["DISTRICT"] = ConditionService.GetDistrictLists();

                if (ContentHelpers.IsNotnull(id) && Convert.ToInt32(id) > 0)
                {
                    List<AppraisalDetailModel> modelList = AppraisalService.GetAppraisalDetail(0, Convert.ToInt32(id), "");
                    if (ContentHelpers.IsNotnull(modelList) && modelList.Count > 0)
                    {
                        model = modelList[0];
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            
            return View(model);
        }

        [HttpPost]
        [Permission]
        public ActionResult ManageAssetDoc(AppraisalDetailModel model)//เอกสารสิทธิ์
        {
            ViewData["TYPE_OF_DOCUMENT"] = ConditionService.GetFilterLists("TYPE_OF_DOCUMENT");
            ViewData["CONDITION_LAND"] = ConditionService.GetFilterLists("CONDITION_LAND");
            ViewData["PROVINCE"] = ConditionService.GetProvinceLists();
            ViewData["AMPHUR"] = ConditionService.GetAmphurLists();
            ViewData["DISTRICT"] = ConditionService.GetDistrictLists();
            try
            {
                if (ModelState.IsValid)
                {
                    string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                    var process = AppraisalService.MngAppraisalDetail(model, userName);
                    if (process)
                    {
                        List<AppraisalDetailModel> modelList = AppraisalService.GetAppraisalDetail(0, model.assets_detail_id, userName);
                        if (ContentHelpers.IsNotnull(modelList) && modelList.Count > 0)
                        {
                            model = modelList[0];
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("MESSAGE", "ไม่สามารถเพิ่มหรือแก้ไขข้อมูลได้ กรุณาตรวจสอบ");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View(model);
        }

        public ActionResult ManageAssetDocPic()//รูปเอกสารสิทธิ์
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManageAssetDocPic(UploadPictureAssetModel model, HttpPostedFileBase[] MultipleFiles)//รูปเอกสารสิทธิ์
        {
            model.appraisal_assets_id = 1;
            if (MultipleFiles != null)
            {
                foreach (var fileBase in MultipleFiles)
                {
                    if (fileBase != null && fileBase.ContentLength > 0)
                    {
                        try
                        {
                            string pathPic = Server.MapPath("~/Images/Document/" + model.appraisal_assets_id);
                            if (!Directory.Exists(pathPic))
                            {
                                Directory.CreateDirectory(pathPic);
                            }
                            string savePath = Path.Combine(pathPic,
                                          Path.GetFileName(fileBase.FileName));
                            fileBase.SaveAs(savePath);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("",ex.Message.ToString());
                        }
                    }
                }

            } 
          
            return View();
        }

        public ActionResult ManageAssetPic()//รูปทรัพย์สิน
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ManageAssetPic(UploadPictureAssetModel model, HttpPostedFileBase[] MultipleFiles)//รูปทรัพย์สิน
        {
            model.appraisal_assets_id = 1;
            if (MultipleFiles != null)
            {
                foreach (var fileBase in MultipleFiles)
                {
                    if (fileBase != null && fileBase.ContentLength > 0)
                    {
                        try
                        {
                            string pathPic = Server.MapPath("~/Images/Asset/" + model.appraisal_assets_id);
                            if (!Directory.Exists(pathPic))
                            {
                                Directory.CreateDirectory(pathPic);
                            }
                            string savePath = Path.Combine(pathPic,
                                          Path.GetFileName(fileBase.FileName));
                            fileBase.SaveAs(savePath);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", ex.Message.ToString());
                        }
                    }
                }

            } 
            return View();
        }
        
        public ActionResult ManageCompareAssetPic()//รูปข้อมูลเทียบ
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ManageCompareAssetPic(UploadPictureAssetModel model, HttpPostedFileBase[] MultipleFiles)//รูปข้อมูลเทียบ
        {
            model.appraisal_assets_id = 1;
            if (MultipleFiles != null)
            {
                foreach (var fileBase in MultipleFiles)
                {
                    if (fileBase != null && fileBase.ContentLength > 0)
                    {
                        try
                        {
                            string pathPic = Server.MapPath("~/Images/Compare/" + model.appraisal_assets_id);
                            if (!Directory.Exists(pathPic))
                            {
                                Directory.CreateDirectory(pathPic);
                            }
                            string savePath = Path.Combine(pathPic,
                                          Path.GetFileName(fileBase.FileName));
                            fileBase.SaveAs(savePath);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", ex.Message.ToString());
                        }
                    }
                }

            } 
            return View();
        }

        [Permission]
        public ActionResult ManageMaterial(string id)//สิ่งปลูกสร้าง
        {
            LocationAssetModel model = new LocationAssetModel();
            try
            {
                ViewData["BUILDING_TYPE"] = ConditionService.GetFilterLists("BUILDING_TYPE");
                ViewData["CONDITION_BUILDING"] = ConditionService.GetFilterLists("CONDITION_BUILDING");
                ViewData["STRUCTURE"] = ConditionService.GetFilterLists("STRUCTURE");
                ViewData["MATERIALS"] = ConditionService.GetFilterLists("MATERIALS");
                ViewData["CEILING"] = ConditionService.GetFilterLists("CEILING");
                ViewData["EX-INTERIOR_WALLS"] = ConditionService.GetFilterLists("EX-INTERIOR_WALLS");
                ViewData["STAIR"] = ConditionService.GetFilterLists("STAIR");

                if (ContentHelpers.IsNotnull(id) && Convert.ToInt32(id) > 0)
                {
                    List<LocationAssetModel> modelList = AppraisalService.GetLocationAsset(0, Convert.ToInt32(id), "");
                    if (ContentHelpers.IsNotnull(modelList) && modelList.Count > 0)
                    {
                        model = modelList[0];
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View(model);
        }

        [HttpPost]
        [Permission]
        public ActionResult ManageMaterial(LocationAssetModel model)//สิ่งปลูกสร้าง
        {
            try
            {
                ViewData["BUILDING_TYPE"] = ConditionService.GetFilterLists("BUILDING_TYPE");
                ViewData["CONDITION_BUILDING"] = ConditionService.GetFilterLists("CONDITION_BUILDING");
                ViewData["STRUCTURE"] = ConditionService.GetFilterLists("STRUCTURE");
                ViewData["MATERIALS"] = ConditionService.GetFilterLists("MATERIALS");
                ViewData["CEILING"] = ConditionService.GetFilterLists("CEILING");
                ViewData["EX-INTERIOR_WALLS"] = ConditionService.GetFilterLists("EX-INTERIOR_WALLS");
                ViewData["STAIR"] = ConditionService.GetFilterLists("STAIR");

                if (ModelState.IsValid)
                {
                    string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                    var process = AppraisalService.MngLocationAsset(model, userName);
                    if (process)
                    {
                        List<LocationAssetModel> modelList = AppraisalService.GetLocationAsset(0, model.appraisal_assets_id, userName);
                        if (ContentHelpers.IsNotnull(modelList) && modelList.Count > 0)
                        {
                            model = modelList[0];
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("MESSAGE", "ไม่สามารถเพิ่มหรือแก้ไขข้อมูลได้ กรุณาตรวจสอบ");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View(model);
        }

        [Permission]
        public ActionResult ManageCompareAsset(string id)//ตารางเปรียบเทียบ
        {
            List<CompareAssetModel> modelList = new List<CompareAssetModel>();
            try
            {
                ViewData["BUILDING_TYPE"] = ConditionService.GetFilterLists("BUILDING_TYPE");
                ViewData["CONDITION_BUILDING"] = ConditionService.GetFilterLists("CONDITION_BUILDING");
                ViewData["STRUCTURE"] = ConditionService.GetFilterLists("STRUCTURE");
                ViewData["MATERIALS"] = ConditionService.GetFilterLists("MATERIALS");
                ViewData["CEILING"] = ConditionService.GetFilterLists("CEILING");
                ViewData["EX-INTERIOR_WALLS"] = ConditionService.GetFilterLists("EX-INTERIOR_WALLS");
                ViewData["STAIR"] = ConditionService.GetFilterLists("STAIR");

                if (ContentHelpers.IsNotnull(id) && Convert.ToInt32(id) > 0)
                {
                    modelList = AppraisalService.GetCompareAsset(0, Convert.ToInt32(id), "");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View(modelList);
        }

        [HttpPost]
        [Permission]
        public ActionResult ManageCompareAsset(CompareAssetModel model)//ตารางเปรียบเทียบ
        {
            try
            {

            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View(model);
        }

        public ActionResult ManageOtherDetail()//รายละเอียดเพิ่มเติม
        {
            return View();
        }

        public ActionResult ManagePrice()//สรุปราคา
        {
            return View();
        }
    }
}
