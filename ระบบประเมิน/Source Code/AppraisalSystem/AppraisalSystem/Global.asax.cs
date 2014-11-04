using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AppraisalSystem.Utility;
using AppraisalSystem.Models;

namespace AppraisalSystem
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public IFormsAuthenticationService FormsService { get; set; }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Account", action = "LogOn", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            System.Diagnostics.Debug.WriteLine("Session_Start");
            string sessionId = Session.SessionID;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Session_End");
            string userName = ContentHelpers.Decode(Convert.ToString(Session["UserName"]));
            FormsService.SignOut(userName);
            Session.Abandon();
        }
    }
}