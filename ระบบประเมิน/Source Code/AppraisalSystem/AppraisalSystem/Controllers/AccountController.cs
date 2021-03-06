﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AppraisalSystem.Models;
using System.Collections;
using AppraisalSystem.Utility;

namespace AppraisalSystem.Controllers
{
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            // Clear Data that set for Layout menu
            ContentHelpers.clearParamForSetMenu();

            ViewBag.Title = String.Format(Resources.ConfigResource.PROJECT_TITLE_NAME, " : เข้าใช้งาน");
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            ViewBag.Title = String.Format(Resources.ConfigResource.PROJECT_TITLE_NAME, " : เข้าใช้งาน");
            try
            {
                if (ModelState.IsValid)
                {
                    Hashtable process = MembershipService.ValidateUser(model.UserName, model.Password);
                    if (Convert.ToBoolean(process["Status"]))
                    {
                        FormsService.SignIn(model.UserName, process["userId"].ToString(), model.RememberMe);
                        switch (Convert.ToString(process["RoleCode"]))
                        {
                            case "ADMIN":
                                return RedirectToAction("Manage", "Backend");
                            case "VALUER":
                                return RedirectToAction("Manage", "Home");
                            case "CHECKER":
                                return RedirectToAction("ManageSearch", "Search");
                            default:
                                ViewData["alert"] = "ไม่มีสิทธิ์ในการเข้าใช้งานระบบ";
                                break;
                        }
                    }
                    else
                    {
                        ViewData["alert"] = ContentHelpers.getAlertBox(DataInfo.AlertStatusId.WARNING, Convert.ToString(process["Message"]));
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            if (Request.IsAuthenticated)
            {
                FormsService.SignOut(userName);
            }
            return RedirectToAction("LogOn", "Account");
        }

        public ActionResult SessionLogOff(string userName)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            FormsService.SessionSignOut(userName);
            return RedirectToAction("LogOn", "Account");
        }
    }
}
