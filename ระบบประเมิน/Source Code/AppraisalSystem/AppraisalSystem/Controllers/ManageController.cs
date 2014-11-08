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
        public ActionResult ManageAssetDetail(string appraisalID, string appraisalManageType)//ข้อมูลที่ตั้งทรัพย์สิน
        {
            setAssetDetail(); //ระบุ filter ในหน้า View

            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            TempData["appraisalManageType"] = appraisalManageType != string.Empty ? ContentHelpers.Decode(appraisalManageType) : "";

            int thisID = appraisalID != null?Convert.ToInt32(ContentHelpers.Decode(appraisalID)):0;
            if (!string.IsNullOrEmpty(appraisalManageType))
            {
                string thisManageType = ContentHelpers.Decode(appraisalManageType);

                //ระบุ id user, job code, ความสามารถ update/view ของ user ลง Tempdata
                setManageDetail(thisID, thisManageType);

                AppraisalJobModel Model = LoadAssetDetail(thisID, userName);

                return View(Model);
            }
            else
            {
                if (thisID != null) 
                {
                    TempData["appraisalManageType"] = "u";
                }
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
                        model.district_id = 1;
                        Hashtable process = AppraisalService.MngAppraisalJob(model, userName);

                        if (Convert.ToBoolean(process["Status"]))
                        {
                            if (process["appraisalID"] != null)
                            {
                                //ระบุ id user, job code, ความสามารถ update/view ของ user ลง Tempdata
                                setManageDetail(Convert.ToInt32(process["appraisalID"]), appraisalManageType);

                                return RedirectToAction(
                                    "ManageAssetMap",
                                    new RouteValueDictionary(new
                                    {
                                        appraisalID = ContentHelpers.Encode(process["appraisalID"].ToString()),
                                        appraisalManageType = ContentHelpers.Encode(appraisalManageType)
                                    })
                                );
                            }
                        }
                        else
                        {
                            TempData["AppraisalManageType"] = appraisalManageType;
                            ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.ERROR, "จัดการข้อมูลไม่สำเร็จ!");
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
        public ActionResult ManageAssetMap(string appraisalID, string appraisalManageType)//แผนที่
        {
            int thisID = Convert.ToInt32(ContentHelpers.Decode(appraisalID));
            getAppraisalAssetCode(thisID);
            string thisManageType = ContentHelpers.Decode(appraisalManageType);
            TempData["appraisalManageType"] = appraisalManageType != string.Empty ? ContentHelpers.Decode(appraisalManageType) : "";

            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            MapAssetModel model = new MapAssetModel();

            List<MapAssetModel> listMap = AppraisalService.GetMapAsset(0, thisID, userName);
            if (listMap != null)
            {
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
                                   "ManageAssetDoc",
                                   new RouteValueDictionary(new
                                   {
                                       appraisalID = ContentHelpers.Encode(model.appraisal_assets_id.ToString()),
                                       appraisalManageType = ContentHelpers.Encode(appraisalManageType)
                                   })
                               );
                    }
                    else
                    {
                        ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.ERROR, "จัดการข้อมูลไม่สำเร็จ!");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            getAppraisalAssetCode(model.appraisal_assets_id);
            return View();
        }

        [Permission]
        public ActionResult ManageAssetDoc(string appraisalID, string appraisalManageType)//เอกสารสิทธิ์
        {
            AppraisalDetailModel model = new AppraisalDetailModel();
            //model.appraisal_assets_id = Convert.ToInt32(TempData["AppraisalID"]);
            //TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);

            int thisID = Convert.ToInt32(ContentHelpers.Decode(appraisalID));
            model.appraisal_assets_id = thisID;
            getAppraisalAssetCode(thisID);
            string thisManageType = ContentHelpers.Decode(appraisalManageType);
            TempData["appraisalManageType"] = appraisalManageType != string.Empty ? ContentHelpers.Decode(appraisalManageType) : "";

            try
            {
                setAssetDoc();

                if (ContentHelpers.IsNotnull(thisID) && thisID > 0)
                {
                    List<AppraisalDetailModel> modelList = AppraisalService.GetAppraisalDetail(0, thisID, "");
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
        public ActionResult ManageAssetDoc(AppraisalDetailModel model, string appraisalManageType)//เอกสารสิทธิ์
        {
          //  TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
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
                            return RedirectToAction(
                                   "ManageAssetDocPic",
                                   new RouteValueDictionary(new
                                   {
                                       appraisalID = ContentHelpers.Encode(model.appraisal_assets_id.ToString()),
                                       appraisalManageType = ContentHelpers.Encode(appraisalManageType)
                                   })
                               );
                        }
                    }
                    else
                    {
                        ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.ERROR, "จัดการข้อมูลไม่สำเร็จ!");
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
        public ActionResult ManageAssetDocPic(string appraisalID, string appraisalManageType)//รูปเอกสารสิทธิ์
        {

            int thisID = Convert.ToInt32(ContentHelpers.Decode(appraisalID));
            getAppraisalAssetCode(thisID);
            string thisManageType = ContentHelpers.Decode(appraisalManageType);
            TempData["appraisalManageType"] = appraisalManageType != string.Empty ? ContentHelpers.Decode(appraisalManageType) : "";

            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));

            List<UploadPictureAssetModel> listImages = AppraisalService.GetUploadPictureAsset(0, 1, thisID, userName);

            if (listImages == null)
            {
                listImages = new List<UploadPictureAssetModel>();
                for (int i = 0; i < 3; i++)
                {
                    UploadPictureAssetModel image = new UploadPictureAssetModel();
                    image.appraisal_assets_id = thisID;
                    listImages.Add(image);
                }
            }
            return View(listImages);
        }

        [HttpPost]
        [Permission]
        public ActionResult ManageAssetDocPic(List<UploadPictureAssetModel> models, HttpPostedFileBase[] MultipleFiles, string appraisalManageType)//รูปเอกสารสิทธิ์
        {
            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            Boolean process = true;
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

                if (model.image_assets_id == 0 || pathPic != "")
                {
                    process = false;
                    model.image_path = pathPic;
                    model.file_name = fileName;
                    model.upload_type_id = 1; //รูปภาพเอกสารสิทธิ์
                    model.sequence = count;
                    process = AppraisalService.MngUploadPicture(model, userName);
                }

            }
            if (Convert.ToBoolean(process))
            {
                return RedirectToAction(
                           "ManageAssetPic",
                           new RouteValueDictionary(new
                           {
                               appraisalID = ContentHelpers.Encode(appraisalID.ToString()),
                               appraisalManageType = ContentHelpers.Encode(appraisalManageType)
                           })
                       );
            }
            else
            {
                ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.WARNING, "จัดการข้อมูลไม่สำเร็จ!");
            }

            return View();
        }


        [Permission]
        public ActionResult ManageAssetPic(string appraisalID, string appraisalManageType)//รูปทรัพย์สิน
        {

            int thisID = Convert.ToInt32(ContentHelpers.Decode(appraisalID));
            getAppraisalAssetCode(thisID);
            string thisManageType = ContentHelpers.Decode(appraisalManageType);
            TempData["appraisalManageType"] = appraisalManageType != string.Empty ? ContentHelpers.Decode(appraisalManageType) : "";

            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            List<UploadPictureAssetModel> listImages = AppraisalService.GetUploadPictureAsset(0, 2, thisID, userName);

            if (listImages == null)
            {
                listImages = new List<UploadPictureAssetModel>();
                for (int i = 0; i < 3; i++)
                {
                    UploadPictureAssetModel image = new UploadPictureAssetModel();
                    image.appraisal_assets_id = thisID;
                    listImages.Add(image);
                }
            }
            return View(listImages);
        }


        [HttpPost]
        [Permission]
        public ActionResult ManageAssetPic(List<UploadPictureAssetModel> models, HttpPostedFileBase[] MultipleFiles, string appraisalManageType)//รูปทรัพย์สิน
        {
            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            Boolean process = true;
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
                    process = false;
                    model.image_path = pathPic;
                    model.file_name = fileName;
                    model.upload_type_id = 2; //รูปภาพทรัพย์สิน
                    model.sequence = count;
                    process = AppraisalService.MngUploadPicture(model, userName);
                }

            }
            if (Convert.ToBoolean(process))
            {
                return RedirectToAction(
                           "ManageCompareAssetPic",
                           new RouteValueDictionary(new
                           {
                               appraisalID = ContentHelpers.Encode(appraisalID.ToString()),
                               appraisalManageType = ContentHelpers.Encode(appraisalManageType)
                           })
                       );
            }
            else
            {
                ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.ERROR, "จัดการข้อมูลไม่สำเร็จ!");
            }

            return View();
        }

        [Permission]
        public ActionResult ManageCompareAssetPic(string appraisalID, string appraisalManageType)//รูปข้อมูลเทียบ
        {
            int thisID = Convert.ToInt32(ContentHelpers.Decode(appraisalID));
            getAppraisalAssetCode(thisID);
            string thisManageType = ContentHelpers.Decode(appraisalManageType);
            TempData["appraisalManageType"] = appraisalManageType != string.Empty ? ContentHelpers.Decode(appraisalManageType) : "";

            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            List<UploadPictureAssetModel> listImages = AppraisalService.GetUploadPictureAsset(0, 3, thisID, userName);

            if (listImages == null)
            {
                listImages = new List<UploadPictureAssetModel>();
                for (int i = 0; i < 3; i++)
                {
                    UploadPictureAssetModel image = new UploadPictureAssetModel();
                    image.appraisal_assets_id = thisID;
                    listImages.Add(image);
                }
            }
            return View(listImages);
        }

        [HttpPost]
        [Permission]
        public ActionResult ManageCompareAssetPic(List<UploadPictureAssetModel> models, HttpPostedFileBase[] MultipleFiles, string appraisalManageType)//รูปข้อมูลเทียบ
        {
            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            Boolean process = true;
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
                    process = false;
                    model.image_path = pathPic;
                    model.file_name = fileName;
                    model.upload_type_id = 3; //รูปภาพเปรียบเทียบ
                    model.sequence = count;
                    process = AppraisalService.MngUploadPicture(model, userName);
                }

            }
            if (Convert.ToBoolean(process))
            {
                return RedirectToAction(
                           "ManageMaterial",
                           new RouteValueDictionary(new
                           {
                               appraisalID = ContentHelpers.Encode(appraisalID.ToString()),
                               AssetManageType = ContentHelpers.Encode(appraisalManageType)
                           })
                       );
            }
            else
            {
                ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.ERROR, "จัดการข้อมูลไม่สำเร็จ!");
            }
            return View();
        }

        [Permission]
        public ActionResult ManageMaterial(string appraisalID, string appraisalManageType)//สิ่งปลูกสร้าง
        {
            LocationAssetModel model = new LocationAssetModel();
            //model.appraisal_assets_id = Convert.ToInt32(TempData["AppraisalID"]);
            //sTempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);

            int thisID = Convert.ToInt32(ContentHelpers.Decode(appraisalID));
            model.appraisal_assets_id = thisID;
            getAppraisalAssetCode(thisID);
            string thisManageType = ContentHelpers.Decode(appraisalManageType);
            TempData["appraisalManageType"] = appraisalManageType != string.Empty ? ContentHelpers.Decode(appraisalManageType) : "";

            try
            {
                setMaterial();

                if (ContentHelpers.IsNotnull(thisID) && thisID > 0)
                {
                    List<LocationAssetModel> modelList = AppraisalService.GetLocationAsset(0, thisID, "");
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
        public ActionResult ManageMaterial(LocationAssetModel model, string appraisalManageType)//สิ่งปลูกสร้าง
        {
          //  TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
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
                           return RedirectToAction(
                          "ManageCompareAsset",
                          new RouteValueDictionary(new
                          {
                              appraisalID = ContentHelpers.Encode(model.appraisal_assets_id.ToString()),
                              appraisalManageType = ContentHelpers.Encode(appraisalManageType)
                          })
                      );
                        }
                    }
                    else
                    {
                        ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.ERROR, "จัดการข้อมูลไม่สำเร็จ!");
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
        public ActionResult ManageCompareAsset(string appraisalID, string appraisalManageType)//ตารางเปรียบเทียบ
        {
            List<CompareAssetModel> modelList = new List<CompareAssetModel>();
           // TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);

            int thisID = Convert.ToInt32(ContentHelpers.Decode(appraisalID));
            getAppraisalAssetCode(thisID);
            string thisManageType = ContentHelpers.Decode(appraisalManageType);
            TempData["appraisalManageType"] = appraisalManageType != string.Empty ? ContentHelpers.Decode(appraisalManageType) : "";


            try
            {
                setCompareAsset();

               /* if (ContentHelpers.IsNotnull(thisID) && Convert.ToInt32(thisID) > 0)
                {
                    modelList = AppraisalService.GetCompareAsset(0, thisID, "");
                }
                else
                {*/
                    modelList = AppraisalService.GetCompareAsset(0, thisID, "");
                    if (modelList == null)
                    {
                        modelList = new List<CompareAssetModel>();
                        for (int i = 1; i < 5; i++)
                        {
                            CompareAssetModel compare = new CompareAssetModel();
                            compare.appraisal_assets_id = thisID;
                            compare.sequence = i;
                            modelList.Add(compare);
                        }
                    }
                //}
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
        public ActionResult ManageCompareAsset(List<CompareAssetModel> modelList, string appraisalManageType)//ตารางเปรียบเทียบ
        {
           // TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
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
                        return RedirectToAction(
                       "ManageOtherDetail",
                       new RouteValueDictionary(new
                       {
                           appraisalID = ContentHelpers.Encode(appraisalAssetId.ToString()),
                           appraisalManageType = ContentHelpers.Encode(appraisalManageType)
                       })
                   );
                    }
                }
                else
                {
                    ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.ERROR, "จัดการข้อมูลไม่สำเร็จ!");
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
        public ActionResult ManageOtherDetail(string appraisalID, string appraisalManageType)//รายละเอียดเพิ่มเติม
        {
            List<CompareDescriptionModel> modelList = new List<CompareDescriptionModel>();

           // TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);

            int thisID = Convert.ToInt32(ContentHelpers.Decode(appraisalID));
            getAppraisalAssetCode(thisID);
            string thisManageType = ContentHelpers.Decode(appraisalManageType);
            TempData["appraisalManageType"] = appraisalManageType != string.Empty ? ContentHelpers.Decode(appraisalManageType) : "";

            try
            {
                if (ContentHelpers.IsNotnull(thisID) && thisID > 0)
                {
                    modelList = AppraisalService.GetCompareDescription(0, thisID, "");
                }
                else
                {
                    if (modelList == null)
                    {
                        modelList = new List<CompareDescriptionModel>();
                        for (int i = 1; i < 5; i++)
                        {
                            CompareDescriptionModel compareDesc = new CompareDescriptionModel();
                            compareDesc.appraisal_assets_id = thisID;
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
        public ActionResult ManageOtherDetail(List<CompareDescriptionModel> modelList, string appraisalManageType)//รายละเอียดเพิ่มเติม
        {
           // TempData["AppraisalCode"] = Convert.ToInt32(TempData["AppraisalCode"]);
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
                            return RedirectToAction(
                        "ManagePrice",
                        new RouteValueDictionary(new
                        {
                            appraisalID = ContentHelpers.Encode(appraisalAssetId.ToString()),
                            appraisalManageType = ContentHelpers.Encode(appraisalManageType)
                        })
                          );
                        }
                    }
                    else
                    {
                        ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.ERROR, "จัดการข้อมูลไม่สำเร็จ!");
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
        public ActionResult ManagePrice(string appraisalID, string appraisalManageType)//สรุปราคา
        {
            return View();
        }

        #region Setting Page
        public void setManageDetail(int appraisalID, string appraisalManageType)
        {
            Session.Add("appraisalID", appraisalID);
            Session.Add("AppraisalManageType", appraisalManageType);
            TempData["AppraisalManageType"] = appraisalManageType;
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
            List<UploadPictureAssetModel> listImages = AppraisalService.GetUploadPictureAsset(imageAssetId, 0, 0, userName);
            foreach (UploadPictureAssetModel model in listImages)
            {
                model.image_path = null;
                model.file_name = null;
                result = AppraisalService.MngUploadPicture(model, userName);
            }

            return result;
        }
        public void getAppraisalAssetCode(int appraisalAssetID)
        {

            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            List<AppraisalJobModel> listJob = AppraisalService.GetAppraisalJob(appraisalAssetID, null, userName);

            foreach (AppraisalJobModel model in listJob)
            {
                TempData["appraisalAssetCode"] = model.appraisal_assets_code;

            }
        }
        #endregion
    }
}
