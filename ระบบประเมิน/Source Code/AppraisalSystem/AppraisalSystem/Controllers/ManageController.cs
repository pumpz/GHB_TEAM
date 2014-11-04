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


        [Permission]
        public ActionResult ManageAssetDoc(string id)//เอกสารสิทธิ์
        {
            AppraisalDetailModel model = new AppraisalDetailModel();
            model.appraisal_assets_id = Convert.ToInt32(TempData["AppraisalID"]);
            TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
            try
            {
                setAssetDoc();

                if (ContentHelpers.IsNotnull(id) && Convert.ToInt32(id) > 0)
                {
                    List<AppraisalDetailModel> modelList = AppraisalService.GetAppraisalDetail(0, Convert.ToInt32(id), "");
                    if (ContentHelpers.IsNotnull(modelList) && modelList.Count > 0)
                    {
                        model = modelList[0];
                    }
                }
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError(String.Empty, ae.Message);
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
            TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
            try
            {
                setAssetDoc();

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
            catch (ArgumentException ae)
            {
                ModelState.AddModelError(String.Empty, ae.Message);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View(model);
        }

        public ActionResult ManageAssetDocPic()//รูปเอกสารสิทธิ์
        {
            // string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            string userName = "system";

            List<UploadPictureAssetModel> listImages = AppraisalService.GetUploadPictureAsset(0,1,userName);

            if (listImages == null)
            {
                listImages = new List<UploadPictureAssetModel>();
                for(int i=0;i<3;i++){
                    UploadPictureAssetModel image = new UploadPictureAssetModel();
                    image.appraisal_assets_id = 1;
                    listImages.Add(image);
                }
            }

            return View(listImages);
        }
        [HttpPost]
        public ActionResult ManageAssetDocPic(List<UploadPictureAssetModel> models, HttpPostedFileBase[] MultipleFiles)//รูปเอกสารสิทธิ์
        {
            // string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            string userName = "system";
            Boolean process = false;
            int count = 0;
            string pathPic = "";
            string fileName = "";
            string savePath = "";
            foreach (UploadPictureAssetModel model in models)
            {
                model.appraisal_assets_id = 1;
                pathPic = "";
                fileName = "";
                savePath = "";
                if (MultipleFiles[count] != null && MultipleFiles[count].ContentLength > 0)
                {
                    try
                    {
                        
                        string path = Server.MapPath("~/Images/Document/" + model.appraisal_assets_id);
                        
                        fileName = MultipleFiles[count].FileName;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        savePath = Path.Combine(path,
                                      Path.GetFileName(MultipleFiles[count].FileName));
                        MultipleFiles[count].SaveAs(savePath);
                        pathPic = "~/Images/Document/" + model.appraisal_assets_id + "/" + fileName;


                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message.ToString());
                    }
                }

                count++;
                model.image_path = pathPic;
                model.file_name = fileName;
                model.upload_type_id = 1; //รูปภาพเอกสารสิทธิ์
                model.sequence = count;
                process = AppraisalService.MngUploadPicture(model, userName);

            }
            if (Convert.ToBoolean(process))
            {
                List<UploadPictureAssetModel> listImages = AppraisalService.GetUploadPictureAsset(0, 1, userName);
                return View(listImages);
            }
            else
            {
                ModelState.AddModelError("", Convert.ToString("Insert Asset image unsuccess."));
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
              model.appraisal_assets_id = Convert.ToInt32(TempData["AppraisalID"]);
              TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
              try
              {
                  setMaterial();
                  
                  if (ContentHelpers.IsNotnull(id) && Convert.ToInt32(id) > 0)
                  {
                      List<LocationAssetModel> modelList = AppraisalService.GetLocationAsset(0, Convert.ToInt32(id), "");
                      if (ContentHelpers.IsNotnull(modelList) && modelList.Count > 0)
                      {
                          model = modelList[0];
                      }
                  }
              }
              catch (ArgumentException ae)
              {
                  ModelState.AddModelError(String.Empty, ae.Message);
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
              TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
              try
              {
                  setMaterial();

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
              catch (ArgumentException ae)
              {
                  ModelState.AddModelError(String.Empty, ae.Message);
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
              List<CompareAssetModel> modelList = null;
              TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
              try
              {
                  setCompareAsset();

                  if (ContentHelpers.IsNotnull(id) && Convert.ToInt32(id) > 0)
                  {
                      modelList = AppraisalService.GetCompareAsset(0, Convert.ToInt32(id), "");
                  }
                  else
                  {
                      if (modelList == null)
                      {
                          modelList = new List<CompareAssetModel>();
                          for (int i = 1; i < 5; i++)
                          {
                              CompareAssetModel compare = new CompareAssetModel();
                              compare.appraisal_assets_id = Convert.ToInt32(TempData["AppraisalID"]);
                              compare.sequence = i;
                              modelList.Add(compare);
                          }
                      }
                  }
              }
              catch (ArgumentException ae)
              {
                  ModelState.AddModelError(String.Empty, ae.Message);
              }
              catch (Exception e)
              {
                  ModelState.AddModelError(String.Empty, e.Message);
              }
              return View(modelList);
          }

          [HttpPost]
          [Permission]
          public ActionResult ManageCompareAsset(List<CompareAssetModel> modelList)//ตารางเปรียบเทียบ
          {
              TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
              try
              {
                  setCompareAsset();

                  if (ModelState.IsValid)
                  {
                      string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                      bool process = false;
                      int appraisalAssetId = 0;
                      int i = 0;
                      foreach (var model in modelList)
                      {
                          model.sequence = i++;
                          appraisalAssetId = model.appraisal_assets_id;
                          process = AppraisalService.MngCompareAsset(model, userName);
                      }

                      if (process)
                      {
                          if (appraisalAssetId > 0)
                          {
                              modelList = AppraisalService.GetCompareAsset(0, appraisalAssetId, userName);
                          }
                      }
                      else
                      {
                          ModelState.AddModelError("MESSAGE", "ไม่สามารถเพิ่มหรือแก้ไขข้อมูลได้ กรุณาตรวจสอบ");
                      }
                  }
              }
              catch (ArgumentException ae)
              {
                  ModelState.AddModelError(String.Empty, ae.Message);
              }
              catch (Exception e)
              {
                  ModelState.AddModelError(String.Empty, e.Message);
              }
              return View(modelList);
          }

          [Permission]
          public ActionResult ManageOtherDetail(string id)//รายละเอียดเพิ่มเติม
          {
              List<CompareDescriptionModel> modelList = null;
              TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
              try
              {
                  if (ContentHelpers.IsNotnull(id) && Convert.ToInt32(id) > 0)
                  {
                      modelList = AppraisalService.GetCompareDescription(0, Convert.ToInt32(id), "");
                  }
                  else
                  {
                      if (modelList == null)
                      {
                          modelList = new List<CompareDescriptionModel>();
                          for (int i = 1; i < 5; i++)
                          {
                              CompareDescriptionModel compareDesc = new CompareDescriptionModel();
                              compareDesc.appraisal_assets_id = Convert.ToInt32(TempData["APPRAISAL_ASSETS_ID"]);
                              compareDesc.sequence = i;
                              modelList.Add(compareDesc);
                          }
                      }
                  }
              }
              catch (ArgumentException ae)
              {
                  ModelState.AddModelError(String.Empty, ae.Message);
              }
              catch (Exception e)
              {
                  ModelState.AddModelError(String.Empty, e.Message);
              }
              return View(modelList);
          }

          [HttpPost]
          [Permission]
          public ActionResult ManageOtherDetail(List<CompareDescriptionModel> modelList)//รายละเอียดเพิ่มเติม
          {
              TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
              try
              {
                  if (ModelState.IsValid)
                  {
                      string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                      bool process = false;
                      int appraisalAssetId = 0;
                      foreach (var model in modelList)
                      {
                          appraisalAssetId = model.appraisal_assets_id;
                          process = AppraisalService.MngCompareDescription(model);
                      }

                      if (process)
                      {
                          if (appraisalAssetId > 0)
                          {
                              modelList = AppraisalService.GetCompareDescription(0, appraisalAssetId, userName);
                          }
                      }
                      else
                      {
                          ModelState.AddModelError("MESSAGE", "ไม่สามารถเพิ่มหรือแก้ไขข้อมูลได้ กรุณาตรวจสอบ");
                      }
                  }
              }
              catch (ArgumentException ae)
              {
                  ModelState.AddModelError(String.Empty, ae.Message);
              }
              catch (Exception e)
              {
                  ModelState.AddModelError(String.Empty, e.Message);
              }
              return View(modelList);
          }

        public ActionResult ManagePrice()//สรุปราคา
        {
            return View();
        }

        #region Setting Page
        public void setAssetDoc()
        {
            ViewData["TYPE_OF_DOCUMENT"] = ConditionService.GetFilterLists("TYPE_OF_DOCUMENT");
            ViewData["CONDITION_LAND"] = ConditionService.GetFilterLists("CONDITION_LAND");
            ViewData["ISCONDITION"] = ConditionService.GetFilterLists("ISCONDITION");
            ViewData["PROVINCE"] = ConditionService.GetProvinceLists();
            ViewData["AMPHUR"] = ConditionService.GetAmphurLists();
            ViewData["DISTRICT"] = ConditionService.GetDistrictLists();
        }

        public void setMaterial()
        {
            ViewData["BUILDING_TYPE"] = ConditionService.GetFilterLists("BUILDING_TYPE");
            ViewData["CONDITION_BUILDING"] = ConditionService.GetFilterLists("CONDITION_BUILDING");
            ViewData["STRUCTURE"] = ConditionService.GetFilterLists("STRUCTURE");
            ViewData["MATERIALS"] = ConditionService.GetFilterLists("MATERIALS");
            ViewData["CEILING"] = ConditionService.GetFilterLists("CEILING");
            ViewData["EX-INTERIOR_WALLS"] = ConditionService.GetFilterLists("EX-INTERIOR_WALLS");
            ViewData["STAIR"] = ConditionService.GetFilterLists("STAIR");
        }

        public void setCompareAsset()
        {
            ViewData["SHAPE_INFORMATION"] = ConditionService.GetFilterLists("SHAPE_INFORMATION");
            ViewData["ENVIRONMENT"] = ConditionService.GetFilterLists("ENVIRONMENT");
            ViewData["CONDITION_LAND"] = ConditionService.GetFilterLists("CONDITION_LAND");
            ViewData["CHARACTERISTICS_ACCESS"] = ConditionService.GetFilterLists("CHARACTERISTICS_ACCESS");
            ViewData["PUBLIC_UTILITY"] = ConditionService.GetFilterLists("PUBLIC_UTILITY");
            ViewData["ISCONDITION"] = ConditionService.GetFilterLists("ISCONDITION");
            ViewData["LEVEL"] = ConditionService.GetFilterLists("LEVEL");
        }

        public void setAssetDetail() 
        {
            ViewData["Province"] = ConditionService.GetProvinceLists();
            ViewData["Amphur"] = ConditionService.GetAmphurLists();
            ViewData["District"] = ConditionService.GetDistrictLists();
            ViewData["AssetType"] = ConditionService.GetFilterLists("").Where(a => a.filter_type_code == "000001").ToList();
            ViewData["AssetmentMethod"] = ConditionService.GetFilterLists("").Where(a => a.filter_type_code == "000002").ToList();
            ViewData["RightOfAccess"] = ConditionService.GetFilterLists("").Where(a => a.filter_type_code == "000003").ToList();
            ViewData["PaintTheTown"] = ConditionService.GetFilterLists("").Where(a => a.filter_type_code == "000004").ToList();
        }

        #endregion
    }
}
