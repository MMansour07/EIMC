using eInvoicing.Service.Helper;
using eInvoicing.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace eInvoicing.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Init();
            Bootstrapper.Initialise();

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();
        }
        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
        //    if (cookie != null && cookie.Value != null)
        //    {
        //        System.Threading.Thread.CurrentThread.CurrentCulture
        //        = new System.Globalization.CultureInfo(cookie.Value);
        //        System.Threading.Thread.CurrentThread.CurrentUICulture
        //        = new System.Globalization.CultureInfo(cookie.Value);
        //    }
        //    else
        //    {
        //        System.Threading.Thread.CurrentThread.CurrentCulture
        //         = new System.Globalization.CultureInfo("en");
        //        System.Threading.Thread.CurrentThread.CurrentUICulture
        //       = new System.Globalization.CultureInfo("en");
        //    }
        //}
    }
}
