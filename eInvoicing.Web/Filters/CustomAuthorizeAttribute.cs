using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eInvoicing.Web.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            var simplePrinciple = (ClaimsPrincipal)HttpContext.Current.User;
            var identity = simplePrinciple?.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var claims = identity?.Claims.ToArray();

            var usernameClaim = identity?.FindFirst(ClaimTypes.Name);
            var username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;
            //if ((httpContext.Request.Url.Segments.Length > 3) && httpContext.Request.Url.Segments[3]?.Replace("/", "") != "documentsubmission")
            //{
            var pages = identity?.FindAll("Page").Select(i => i.Value.ToLower()).ToList();
            var permissions = identity?.FindAll("Permission").Select(i => i.Value).ToList();
            if (!permissions.Contains(httpContext.Request.HttpMethod.ToString()) || ((httpContext.Request.Url.Segments.Length > 3) ? !pages.Contains(httpContext.Request.Url.Segments[3]?.Replace("/", "")) : false))
                return false;
            //}
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Account" },
                    { "action", "Login" }
               });
        }
    }
}