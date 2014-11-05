using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppraisalSystem.Utility;
using AppraisalSystem.Models;
using System.Collections;
using System.Web.Routing;





namespace AppraisalSystem.Controllers
{
    public class BackendController : Controller
    {
        public IMembershipService MembershipService { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }
        //
        // GET: /Backend/

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Manage()
        {

            List<UserModel> users = MembershipService.GetUsers(null);

            return View(users);
        }


        [HttpPost]
        public ActionResult Manage(FormCollection collection)
        {
            string keyword = collection.Get("txtSearchKeyword");
            List<UserModel> users = MembershipService.GetUsers(keyword);

            return View(users);
        }

        public ActionResult ManagePermissions()
        {
            return View();
        }

        // **************************************
        // URL: /Backend/Register
        // **************************************

        public ActionResult Register()
        {
            ViewData["Role"] = MembershipService.GetAllRole();
            return View();
        }

        [HttpPost]
        [Permission]
        public ActionResult Register(RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                     string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                    //string userName = "system";
                    // Attempt to register the user
                    Hashtable process = MembershipService.CreateUser(model, userName);
                    if (Convert.ToBoolean(process["Status"]))
                    {
                        return RedirectToAction("manage", "backend");
                    }
                    else
                    {
                        ModelState.AddModelError("", Convert.ToString(process["Message"]));
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }

            return View(model);
        }

        // **************************************
        // URL: /Backend/Update
        // **************************************

        public ActionResult UpdateUser(int id)
        {
            UserModel user = null;
            RegisterModel reg = new RegisterModel();
            ViewData["Role"] = MembershipService.GetAllRole();
            try
            {
                user = MembershipService.GetUsersByID(id);
                if (ContentHelpers.Isnull(user))
                {
                    ModelState.AddModelError("", "Data not found");
                }
                else
                {
                    reg.CitizenID = user.CitizenID;
                    reg.Email = user.Email;
                    reg.Name = user.Name;
                    reg.Phone = user.Phone;
                    reg.RoleID = user.RoleID;
                    reg.Status = user.Status;
                    reg.UserName = user.UserName;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View(reg);
        }

      
        [HttpPost]
        [Permission]
        public ActionResult UpdateUser(RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                   // string userName = "system";
                    // Attempt to register the user
                    bool process = MembershipService.UpdateUser(model, userName);
                    if (process)
                    {
                        return RedirectToAction("manage", "backend");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Update user unsuccess");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }

            return View(model);
        }

        // **************************************
        // URL: /Backend/Delete
        // **************************************

        [Authorize]
        [Permission]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                if (id > 0)
                {
                    string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                    //string userName ="system";
                    if (MembershipService.DeleteUser(id, userName))
                    {
                        return RedirectToAction("manage", "backend");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lock user unsuccess.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Not request user id.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }

            return View();
        }

        // **************************************
        // URL: /Backend/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [Permission]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                    if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword, userName))
                    {
                        return RedirectToAction("");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }

            return View(model);
        }

        // **************************************
        // URL: /Backend/Lock
        // **************************************

        [Authorize]
        [Permission]
        public ActionResult LockUser(int id,Boolean isLock)
        {
            try
            {
                if (id > 0)
                {
                     string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                    //string userName = "system";
                    if (MembershipService.LockUser(id, isLock, userName))
                    {
                        return RedirectToAction("manage", "backend");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lock user unsuccess.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Not request user id.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }

            return View();
        }

        // **************************************
        // URL: /Backend/GetUserList
        // **************************************

        [Authorize]
        [HttpPost]
        [Permission]
        public ActionResult GetUserList(string keyword)
        {
            try
            {
                string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
                List<UserModel> userList = MembershipService.GetUsers(keyword);
                if (ContentHelpers.IsNotnull(userList) && userList.Count > 0)
                {
                    ViewData["UserList"] = userList;
                }
                else
                {
                    ModelState.AddModelError("", "Search data not found.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }

            return View();
        }
    }
}
