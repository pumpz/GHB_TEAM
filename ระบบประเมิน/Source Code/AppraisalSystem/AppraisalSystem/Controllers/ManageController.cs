using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppraisalSystem.Models;
using System.Collections;
using System.Web.Routing;

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

        public ActionResult ManageAssetPic()//รูปทรัพย์สิน
        {
            return View();
        }

        public ActionResult ManageCompareAssetPic()//รูปข้อมูลเทียบ
        {
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
    }
}
