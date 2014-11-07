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
using System.ComponentModel;
using System.Linq.Expressions;

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

        protected void setCanUpdate(string manageType)
        {
            ViewBag.data = " new {disabled = 'disabled' }";

            switch(manageType)
            {
                case "u": ViewData["Disabled"] = "new {disabled = 'disabled' }"; break;
            }
        }

        protected AppraisalJobModel LoadAssetDetail(int appraisalID, string username) 
        {
            AppraisalJobModel model = new AppraisalJobModel();

            List<AppraisalJobModel> listJob = AppraisalService.GetAppraisalJob(appraisalID, "", username);
            if (listJob != null)
            {
                foreach (AppraisalJobModel job in listJob)
                {
                    model.appraisal_assets_id = job.appraisal_assets_id;
                    model.appraisal_assets_code = job.appraisal_assets_code;
                    model.village = job.village;
                    model.alley = job.alley;
                    model.road = job.road;
                    model.district_id = job.district_id;
                    model.amphur_id = job.amphur_id;
                    model.province_id = job.province_id;
                    model.detailed_location = job.detailed_location;
                    model.asset_type_id = job.asset_type_id;
                    model.assessment_methods_id = job.assessment_methods_id;
                    model.rights_of_access_id = job.rights_of_access_id;
                    model.paint_the_town_id = job.paint_the_town_id;
                }
            }
            return model;
        }

        //
        // GET: /Manage/
        [Permission]
        public ActionResult ManageAssetDetail(string appraisalID, string appraisalCode, string appraisalManageType)//ข้อมูลที่ตั้งทรัพย์สิน
        {
            ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.WARNING, "จัดการข้อมูลไม่สำเร็จ!");
            setAssetDetail(); //ระบุ filter ในหน้า View

            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            TempData["appraisalManageType"] = appraisalManageType != string.Empty?ContentHelpers.Decode(appraisalManageType):"";

            if(!string.IsNullOrEmpty(appraisalManageType))
            {
                int thisID = Convert.ToInt32(ContentHelpers.Decode(appraisalID));
                string thisCode = ContentHelpers.Decode(appraisalCode);
                string thisManageType = ContentHelpers.Decode(appraisalManageType);
            
                //ระบุ id user, job code, ความสามารถ update/view ของ user ลง Tempdata
                setManageDetail(thisID,thisCode, thisManageType); 

                if (thisManageType == "v")
                {
                    Response.Write("View");

                    //Set visible for edit
                    setCanUpdate(thisManageType);

                    //Load Data for set on page
                    AppraisalJobModel Model = LoadAssetDetail(thisID, userName);

                    return View(Model);
                }
                else if (thisManageType == "u")
                {
                    Response.Write("Update");

                    //Set visible for edit
                    setCanUpdate(thisManageType);

                    //Load Data for set on page
                    AppraisalJobModel Model = LoadAssetDetail(thisID, userName);

                    return View(Model);
                }
            }
            else 
            {
                Response.Write("Insert");
            }

            return View();
        }

        [HttpPost]
        [Permission]
        public ActionResult ManageAssetDetail(AppraisalJobModel model, string appraisalManageType)//ข้อมูลที่ตั้งทรัพย์สิน
        {
            try
            {
                if (ModelState.IsValid)
                {
                   if (isFilterAssetDetail(model))
                   {
                    string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));

                        Hashtable process = AppraisalService.MngAppraisalJob(model, userName);

                        if (Convert.ToBoolean(process["Status"]))
                        {
                            if (process["appraisalCode"] != null)
                            {

                                return RedirectToAction(
                                    "ManageAssetMap",
                                    new RouteValueDictionary(new
                                    {
                                        appraisalID = ContentHelpers.Encode(process["appraisalID"].ToString()),
                                        appraisalCode = ContentHelpers.Encode(process["appraisalCode"].ToString()),
                                        AssetManageType = ContentHelpers.Encode(appraisalManageType)
                                    })
                                );
                            }
                        }
                        else
                        {
                            TempData["AppraisalManageType"] = appraisalManageType;
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

        [Permission]
        public ActionResult ManageAssetMap(string appraisalID, string appraisalCode, string appraisalManageType)//แผนที่
        {
            int thisID = Convert.ToInt32(ContentHelpers.Decode(appraisalID));
            getAppraisalAssetCode(thisID);
            string thisManageType = ContentHelpers.Decode(appraisalManageType);
            TempData["appraisalManageType"] = appraisalManageType != string.Empty ? ContentHelpers.Decode(appraisalManageType) : "";

            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            MapAssetModel model = new MapAssetModel();

            List<MapAssetModel> listMap = AppraisalService.GetMapAsset(0, thisID, userName);
            if(listMap !=null){
                foreach (MapAssetModel map in listMap)
                {
                    model.map_assets_id = map.map_assets_id;
                    model.latitude = map.latitude;
                    model.longitude = map.longitude;
                }
            }
            model.appraisal_assets_id = thisID;

            return View(model);
        }

        [HttpPost]
        [Permission]
        public ActionResult ManageAssetMap(MapAssetModel model, string appraisalManageType)//แผนที่
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
                        return RedirectToAction(
                                   "ManageAssetDocPic",
                                   new RouteValueDictionary(new
                                   {
                                       appraisalID = model.appraisal_assets_id,
                                       AssetManageType = appraisalManageType
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

        [Permission]
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
        [Permission]
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

        [Permission]
        public ActionResult ManageAssetPic(int appraisalID, string AssetManageType)//รูปทรัพย์สิน
        {
            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            TempData["AssetManageType"] = AssetManageType;
            List<UploadPictureAssetModel> listImages = AppraisalService.GetUploadPictureAsset(0, 2, appraisalID, userName);

            if (listImages == null)
            {
                listImages = new List<UploadPictureAssetModel>();
                for (int i = 0; i < 3; i++)
                {
                    UploadPictureAssetModel image = new UploadPictureAssetModel();
                    image.appraisal_assets_id = appraisalID;
                    listImages.Add(image);
                }
            }
            return View(listImages);
        }

        [HttpPost]
        [Permission]
        public ActionResult ManageAssetPic(List<UploadPictureAssetModel> models, HttpPostedFileBase[] MultipleFiles, string AssetManageType)//รูปทรัพย์สิน
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

                        string path = Server.MapPath("~/Images/Asset/" + model.appraisal_assets_id);

                        fileName = MultipleFiles[count].FileName;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        savePath = Path.Combine(path,
                                      Path.GetFileName(MultipleFiles[count].FileName));
                        MultipleFiles[count].SaveAs(savePath);
                        pathPic = "~/Images/Asset/" + model.appraisal_assets_id + "/" + fileName;


                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(String.Empty, ex.Message);
                    }
                }

                count++;

                if (model.image_assets_id == 0 || pathPic != "")
                {
                    model.image_path = pathPic;
                    model.file_name = fileName;
                    model.upload_type_id = 2; //รูปภาพทรัพย์สิน
                    model.sequence = count;
                    process = AppraisalService.MngUploadPicture(model, userName);
                }

            }
            if (Convert.ToBoolean(process))
            {
                setAlert(DataInfo.AlertStatusId.COMPLETE, "เพิ่มข้อมูลเรียบร้อยแล้ว!", "ปรับปรุงข้อมูลเรียบร้อยแล้ว!");
                return RedirectToAction(
                           "ManageCompareAssetPic",
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

        [Permission]
        public ActionResult ManageCompareAssetPic(int appraisalID, string AssetManageType)//รูปข้อมูลเทียบ
        {
            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            TempData["AssetManageType"] = AssetManageType;
            List<UploadPictureAssetModel> listImages = AppraisalService.GetUploadPictureAsset(0, 3, appraisalID, userName);

            if (listImages == null)
            {
                listImages = new List<UploadPictureAssetModel>();
                for (int i = 0; i < 3; i++)
                {
                    UploadPictureAssetModel image = new UploadPictureAssetModel();
                    image.appraisal_assets_id = appraisalID;
                    listImages.Add(image);
                }
            }
            return View(listImages);
        }

        [HttpPost]
        [Permission]
        public ActionResult ManageCompareAssetPic(List<UploadPictureAssetModel> models, HttpPostedFileBase[] MultipleFiles, string AssetManageType)//รูปข้อมูลเทียบ
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

                        string path = Server.MapPath("~/Images/Compare/" + model.appraisal_assets_id);

                        fileName = MultipleFiles[count].FileName;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        savePath = Path.Combine(path,
                                      Path.GetFileName(MultipleFiles[count].FileName));
                        MultipleFiles[count].SaveAs(savePath);
                        pathPic = "~/Images/Compare/" + model.appraisal_assets_id + "/" + fileName;


                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(String.Empty, ex.Message);
                    }
                }

                count++;

                if (model.image_assets_id == 0 || pathPic != "")
                {
                    model.image_path = pathPic;
                    model.file_name = fileName;
                    model.upload_type_id = 3; //รูปภาพเปรียบเทียบ
                    model.sequence = count;
                    process = AppraisalService.MngUploadPicture(model, userName);
                }

            }
            if (Convert.ToBoolean(process))
            {
                setAlert(DataInfo.AlertStatusId.COMPLETE, "เพิ่มข้อมูลเรียบร้อยแล้ว!", "ปรับปรุงข้อมูลเรียบร้อยแล้ว!");
                return RedirectToAction(
                           "ManageCompareAssetPic",
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

        [Permission]
        public ActionResult ManagePrice()//สรุปราคา
        {
            return View();
        }

        #region Setting Page
        public void setManageDetail(int appraisalID, string appraisalCode, string appraisalManageType)
        {
            Session.Add("appraisalID", appraisalID); 
            Session.Add("AppraisalManageType", appraisalManageType); 
            TempData["AppraisalCode"] = appraisalCode;
            TempData["AppraisalManageType"] = appraisalManageType;
        }

        protected void setAlert(DataInfo.AlertStatusId status, string insertMsg, string editMsg)
        {
            string assetManageType = TempData["AppraisalManageType"] != null ? TempData["AppraisalManageType"].ToString() : "";
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

        public Boolean removeImgDB(int imageAssetId)
        {
            Boolean result = false;
            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            List<UploadPictureAssetModel> listImages = AppraisalService.GetUploadPictureAsset(imageAssetId,0, 0, userName);
              foreach (UploadPictureAssetModel model in listImages){
                  model.image_path = null;
                  model.file_name = null;
               result=  AppraisalService.MngUploadPicture(model, userName);
            }

              return result;
        }
        public void getAppraisalAssetCode(int appraisalAssetID)
        {

            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            List<AppraisalJobModel> listJob = AppraisalService.GetAppraisalJob(appraisalAssetID, null, userName);

            foreach (AppraisalJobModel model in listJob)
            {
                TempData["appraisalCode"] = model.appraisal_assets_code;

            }
        }
        #endregion
    }
}
