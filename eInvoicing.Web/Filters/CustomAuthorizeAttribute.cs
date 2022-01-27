using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;

namespace eInvoicing.Web.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                logger.Info(httpContext);

                var simplePrinciple = (ClaimsPrincipal)HttpContext.Current.User;
                var identity = simplePrinciple?.Identity as ClaimsIdentity;
                
                logger.Info(identity);


                if (identity == null)
                    return false;

                if (!identity.IsAuthenticated)
                    return false;

                var claims = identity?.Claims.ToArray();

                var usernameClaim = identity?.FindFirst(ClaimTypes.Name);
                var username = usernameClaim?.Value;

                logger.Info(username);


                if (string.IsNullOrEmpty(username))
                    return false;

                //var permissions = identity?.FindAll("Permission").Select(i => i.Value).ToList();
                //if (httpContext.Request.RequestContext.RouteData.Values["action"].ToString().ToLower() != "index" && 
                //    !permissions.Contains(httpContext.Request.RequestContext.RouteData.Values["action"].ToString().ToLower()))
                //    return false;

                return true;
            }
            catch (Exception ex)
            {

                logger.Error(ex.Message);
                return false;
            }
            
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
            //filterContext.Result = new RedirectToRouteResult(
            //   new RouteValueDictionary
            //   {
            //        { "controller", "Account" },
            //        { "action", "Login" }
            //   });
        }
    }
}