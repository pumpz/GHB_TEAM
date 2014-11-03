using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppraisalSystem.Models;
using System.Collections;
using System.Web.Routing;
using System.IO;

namespace AppraisalSystem.Controllers
{
    public class ManageController : Controller
    {

        public IAppraisalService AppraisalService { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            if (AppraisalService == null) { AppraisalService = new AppraisalService(); }

            base.Initialize(requestContext);
        }
        //
        // GET: /Manage/

        public ActionResult ManageAssetDetail()//ข้อมูลที่ตั้งทรัพย์สิน
        {
            setAssetDetail();

            return View();
        }

        [HttpPost]
        public ActionResult ManageAssetDetail(AppraisalJobModel model)//ข้อมูลที่ตั้งทรัพย์สิน
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userName = "system";
                    Hashtable process = AppraisalService.MngAppraisalJob(model, userName);
                    if (Convert.ToBoolean(process["Status"]))
                    {
                        Response.Write("<script>alert('complete+');</script>");
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("", Convert.ToString("Insert detail not success."));
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }

            setAssetDetail();

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


        public ActionResult ManageAssetDoc()//เอกสารสิทธิ์
        {
            return View();
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

        public ActionResult ManageMaterial()//สิ่งปลูกสร้าง
        {
            return View();
        }

        public ActionResult ManageCompareAsset()//ตารางเปรียบเทียบ
        {
            return View();
        }

        public ActionResult ManageOtherDetail()//รายละเอียดเพิ่มเติม
        {
            return View();
        }

        public ActionResult ManagePrice()//สรุปราคา
        {
            return View();
        }

        public void setAssetDetail() 
        {
            setProvince();
            setAmphur();
            setDistrict();
            setFilterAssetType();
            setFilterAssetmentMethod();
            setFilterRightOfAccess();
            setFilterPaintTheTown();
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

        public void setProvince()
        {
            ConditionService model = new ConditionService();

            ProvinceModel item = new ProvinceModel();
            item.province_id = -1;
            item.province_name = "โปรดเลือก";

            List<ProvinceModel> modelList = model.GetProvinceLists();
            modelList.Insert(0, item);

            ViewData["Province"] = modelList;
        }

        public void setFilterAssetType()
        {
            ConditionService model = new ConditionService();

            FilterModel item = new FilterModel();
            item.filter_type_code = "";
            item.filter_text = "โปรดเลือก";

            
            List<FilterModel> modelList = model.GetFilterLists().Where(a=>a.filter_type_code == "000001").ToList();
            modelList.Insert(0, item);

            ViewData["AssetType"] = modelList;
        }

        public void setFilterAssetmentMethod()
        {
            ConditionService model = new ConditionService();

            FilterModel item = new FilterModel();
            item.filter_type_code = "";
            item.filter_text = "โปรดเลือก";


            List<FilterModel> modelList = model.GetFilterLists().Where(a => a.filter_type_code == "000002").ToList();
            modelList.Insert(0, item);

            ViewData["AssetmentMethod"] = modelList;
        }

        public void setFilterRightOfAccess()
        {
            ConditionService model = new ConditionService();

            FilterModel item = new FilterModel();
            item.filter_type_code = "";
            item.filter_text = "โปรดเลือก";


            List<FilterModel> modelList = model.GetFilterLists().Where(a => a.filter_type_code == "000003").ToList();
            modelList.Insert(0, item);

            ViewData["RightOfAccess"] = modelList;
        }

        public void setFilterPaintTheTown()
        {
            ConditionService model = new ConditionService();

            FilterModel item = new FilterModel();
            item.filter_type_code = "";
            item.filter_text = "โปรดเลือก";


            List<FilterModel> modelList = model.GetFilterLists().Where(a => a.filter_type_code == "000004").ToList();
            modelList.Insert(0, item);

            ViewData["PaintTheTown"] = modelList;
        }
    }
}
