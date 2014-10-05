using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppraisalSystem.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Manage/

        public ActionResult ManageAssetDetail()//ข้อมูลที่ตั้งทรัพย์สิน
        {
            return View();
        }

        public ActionResult ManageAssetMap()//แผนที่
        {
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
