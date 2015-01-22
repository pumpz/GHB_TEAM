using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MFUSurvey.Models;

namespace MFUSurvey.Controllers
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
            ViewBag.Title = Resources.System_Config.TITLE_HOME_EN;
            this.LoadDataDefaultPage();
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            this.LoadDataDefaultPage();
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
                //if (MembershipService.ValidateUser(model.Email, model.Password))
                //{
                //    FormsService.SignIn(model.Email, false);
                //    if (Url.IsLocalUrl(returnUrl))
                //    {
                //        return Redirect(returnUrl);
                //    }
                //    else
                //    {
                //        return RedirectToAction("Index", "Home");
                //    }
                //}
                //else
                //{
                //    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                //}
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void LoadDataDefaultPage()
        {
            ViewBag.Semester = AccountCondition.GetSemester();
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        // **************************************
        // URL: /Account/ForgotPassword
        // **************************************
        public ActionResult ForgotPassword()
        {
            ViewBag.Title = Resources.System_Config.TITLE_FOR_GOT_PASSWORD_EN;
            return View();
        }
    }
}
