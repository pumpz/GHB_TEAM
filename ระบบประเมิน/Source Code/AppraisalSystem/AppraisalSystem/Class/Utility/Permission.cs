using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace AppraisalSystem.Utility
{
    public class Permission : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (ContentHelpers.Isnull(HttpContext.Current.Session["UserID"]) || 
                Convert.ToInt32(HttpContext.Current.Session["UserID"]) <= 0)
            {
                // check if a new session id was generated
                filterContext.Result = new RedirectResult("~/Account/LogOn");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
