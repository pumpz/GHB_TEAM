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

        public ActionResult ManageAssetDetail(string id, string manageType)//ข้อมูลที่ตั้งทรัพย์สิน
        {
            setAssetDetail(); //ระบุ filter ในหน้า View
            setManageDetail(id, manageType); //ระบุid user, job code, ความสามารถ update/view ของ user ลง Tempdata

            /* if (manageType == "v")
             {
                 set readonly   
             }*/
            return View();
        }

        [HttpPost]
        public ActionResult ManageAssetDetail(AppraisalJobModel model, string AssetManageType)//ข้อมูลที่ตั้งทรัพย์สิน
        {
            ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.WARNING, "จัดการข้อมูลไม่สำเร็จ!");
            try
            {
                if (ModelState.IsValid)
                {
                   if (isFilterAssetDetail(model))
                   {
                    string userName = TempData["UserName"].ToString();

                        Hashtable process = AppraisalService.MngAppraisalJob(model, userName);

                        if (Convert.ToBoolean(process["Status"]))
                        {
                            if (process["appraisalID"] != null)
                            {

                                return RedirectToAction(
                                    "ManageAssetMap",
                                    new RouteValueDictionary(new
                                    {
                                        appraisalID = Convert.ToInt32(process["appraisalID"].ToString()),
                                        AssetManageType = AssetManageType
                                    })
                                );
                                //return JavaScript("alert('จัดการข้อมูลเรียบร้อยแล้ว!');");
                                //setAlert(DataInfo.AlertStatusId.COMPLETE, "เพิ่มข้อมูลเรียบร้อยแล้ว!", "ปรับปรุงข้อมูลเรียบร้อยแล้ว!");
                            }
                        }
                        else
                        {
                            ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.WARNING, "จัดการข้อมูลไม่สำเร็จ!");
                        }
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

        protected bool isFilterAssetDetail(AppraisalJobModel model)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(model.appraisal_assets_code))
            {
                ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.ERROR, "กรุณาระบุหมายเลขงาน!");
                isValid = false;
            }
            return isValid;
        }

        public ActionResult ManageAssetMap(int appraisalID, string AssetManageType)//แผนที่
        {
            TempData["AssetManageType"] = AssetManageType;
            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            MapAssetModel model = new MapAssetModel();

            List<MapAssetModel> listMap = AppraisalService.GetMapAsset(0, appraisalID, userName);
            if(listMap !=null){
                foreach (MapAssetModel map in listMap)
                {
                    model.map_assets_id = map.map_assets_id;
                    model.latitude = map.latitude;
                    model.longitude = map.longitude;
                }
            }
            model.appraisal_assets_id = appraisalID;

            return View(model);
        }

        [HttpPost]
        public ActionResult ManageAssetMap(MapAssetModel model, string AssetManageType)//แผนที่
        {
            try
            {
                if (ModelState.IsValid)
                {
                     string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                    // Attempt to register the user
                    Boolean process = AppraisalService.MngMapAsset(model, userName);
                    if (Convert.ToBoolean(process))
                    {
                        setAlert(DataInfo.AlertStatusId.COMPLETE, "เพิ่มข้อมูลเรียบร้อยแล้ว!", "ปรับปรุงข้อมูลเรียบร้อยแล้ว!");
                        return RedirectToAction(
                                   "ManageAssetDocPic",
                                   new RouteValueDictionary(new
                                   {
                                       appraisalID = model.appraisal_assets_id,
                                       AssetManageType = AssetManageType
                                   })
                               );
                    }
                    else
                    {
                        setAlert(DataInfo.AlertStatusId.WARNING, "เพิ่มข้อมูลไม่สำเร็จ!", "ปรับปรุงข้อมูลไม่สำเร็จ!");
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
                            setAlert(DataInfo.AlertStatusId.COMPLETE, "เพิ่มข้อมูลเรียบร้อยแล้ว!", "ปรับปรุงข้อมูลเรียบร้อยแล้ว!");
                        }
                    }
                    else
                    {
                        setAlert(DataInfo.AlertStatusId.WARNING, "ไม่สามารถเพิ่มข้อมูลเรียบร้อยแล้ว!", "ไม่สามารถปรับปรุงข้อมูลเรียบร้อยแล้ว!");
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

        public ActionResult ManageAssetDocPic(int appraisalID, string AssetManageType)//รูปเอกสารสิทธิ์
        {
             string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
             TempData["AssetManageType"] = AssetManageType;
             List<UploadPictureAssetModel> listImages = AppraisalService.GetUploadPictureAsset(0,1, appraisalID, userName);

            if (listImages == null)
            {
                listImages = new List<UploadPictureAssetModel>();
                for(int i=0;i<3;i++){
                    UploadPictureAssetModel image = new UploadPictureAssetModel();
                    image.appraisal_assets_id = appraisalID;
                    listImages.Add(image);
                }
            }
            return View(listImages);
        }

        [HttpPost]
        public ActionResult ManageAssetDocPic(List<UploadPictureAssetModel> models, HttpPostedFileBase[] MultipleFiles, string AssetManageType)//รูปเอกสารสิทธิ์
        {
             string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            Boolean process = false;
            int count = 0;
            string pathPic = "";
            string fileName = "";
            string savePath = "";
            int appraisalID = 0;
         
            foreach (UploadPictureAssetModel model in models)
            {
                pathPic = "";
                fileName = "";
                savePath = "";
         
                appraisalID = model.appraisal_assets_id;
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
                        ModelState.AddModelError(String.Empty, ex.Message);
                    }
                }

                count++;

                if (model.image_assets_id == 0 || pathPic !="")
                {
                    model.image_path = pathPic;
                    model.file_name = fileName;
                    model.upload_type_id = 1; //รูปภาพเอกสารสิทธิ์
                    model.sequence = count;
                    process = AppraisalService.MngUploadPicture(model, userName);
                }

            }
            if (Convert.ToBoolean(process))
            {
                setAlert(DataInfo.AlertStatusId.COMPLETE, "เพิ่มข้อมูลเรียบร้อยแล้ว!", "ปรับปรุงข้อมูลเรียบร้อยแล้ว!");
                return RedirectToAction(
                           "ManageAssetPic",
                           new RouteValueDictionary(new
                           {
                               appraisalID = appraisalID,
                               AssetManageType = AssetManageType
                           })
                       );
            }
            else
            {
                setAlert(DataInfo.AlertStatusId.WARNING, "เพิ่มข้อมูลไม่สำเร็จ!", "ปรับปรุงข้อมูลไม่สำเร็จ!");
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
                            ModelState.AddModelError(String.Empty, ex.Message);
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
                            ModelState.AddModelError(String.Empty, ex.Message);
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
                              setAlert(DataInfo.AlertStatusId.COMPLETE, "เพิ่มข้อมูลเรียบร้อยแล้ว!", "ปรับปรุงข้อมูลเรียบร้อยแล้ว!");
                          }
                      }
                      else
                      {
                          setAlert(DataInfo.AlertStatusId.COMPLETE, "ไม่สามารถเพิ่มข้อมูลเรียบร้อยแล้ว!", "ไม่สามารถปรับปรุงข้อมูลเรียบร้อยแล้ว!");
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
                            setAlert(DataInfo.AlertStatusId.COMPLETE, "เพิ่มข้อมูลเรียบร้อยแล้ว!", "ปรับปรุงข้อมูลเรียบร้อยแล้ว!");
                        }
                    }
                    else
                    {
                        setAlert(DataInfo.AlertStatusId.WARNING, "ไม่สามารถเพิ่มข้อมูลเรียบร้อยแล้ว!", "ไม่สามารถปรับปรุงข้อมูลเรียบร้อยแล้ว!");
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
                              setAlert(DataInfo.AlertStatusId.COMPLETE, "เพิ่มข้อมูลเรียบร้อยแล้ว!", "ปรับปรุงข้อมูลเรียบร้อยแล้ว!");
                          }
                      }
                      else
                      {
                          setAlert(DataInfo.AlertStatusId.WARNING, "ไม่สามารถเพิ่มข้อมูลเรียบร้อยแล้ว!", "ไม่สามารถปรับปรุงข้อมูลเรียบร้อยแล้ว!");
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
        public void setManageDetail(string id, string manageType)
        {
            TempData["UserID"] = ContentHelpers.Decode(Session["UserID"].ToString());
            TempData["AssetID"] = string.IsNullOrEmpty(id) ? "" : ContentHelpers.Decode(id);
            TempData["AssetManageType"] = string.IsNullOrEmpty(manageType) ? "i" : ContentHelpers.Decode(manageType);
        }

        //!!!!
        public void setManagePage()
        {
            string assetManageType = TempData["AssetManageType"] != null ? TempData["AssetManageType"].ToString() : "";
            switch (assetManageType)
            {
                case "i":

                    break;
                case "e":

                    break;
                case "v":
                    TempData["readOnly"] = ",@readonly='true'";

                    break;
            }
        }

        protected void setAlert(DataInfo.AlertStatusId status, string insertMsg, string editMsg)
        {
            string assetManageType = TempData["AssetManageType"] != null ? TempData["AssetManageType"].ToString() : "";
            switch (assetManageType)
            {
                case "i":
                    TempData["alert"] = ContentHelpers.getAlertBox(status, insertMsg);
                    break;
                case "e":
                    TempData["alert"] = ContentHelpers.getAlertBox(status, editMsg);
                    break;
            }
        }

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
            ViewData["AssetType"] = ConditionService.GetFilterLists("ASSET_TYPE");
            ViewData["AssetmentMethod"] = ConditionService.GetFilterLists("ASSESSMENT_METHOD");
            ViewData["RightOfAccess"] = ConditionService.GetFilterLists("RIGHT_OF_ACCESS");
            ViewData["PaintTheTown"] = ConditionService.GetFilterLists("PAINT_THE_TOWN");
        }

        public void removeImgDB(int imageAssetId)
        {
            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            List<UploadPictureAssetModel> listImages = AppraisalService.GetUploadPictureAsset(imageAssetId,0, 0, userName);
              foreach (UploadPictureAssetModel model in listImages){
                  model.image_path = null;
                  model.file_name = null;
                 AppraisalService.MngUploadPicture(model, userName);
            }

           
        }
        #endregion
    }
}
