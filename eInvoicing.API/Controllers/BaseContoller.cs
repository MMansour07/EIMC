using eInvoicing.API.Filters;
using System.Configuration;
using System.Security.Claims;
using System.Web;
using System.Web.Http;


namespace eInvoicing.API.Controllers
{
    //[JwtAuthentication]
    public class BaseController : ApiController
    {
        protected string OnActionExecuting()
        {
            // pre processing
            var simplePrinciple = (ClaimsPrincipal)HttpContext.Current.User;
            var identity = simplePrinciple?.Identity as ClaimsIdentity;
            return ConfigurationManager.ConnectionStrings["EInvoice_" + identity?.FindFirst("BusinessGroup")?.Value?.Replace(" ", "")]?.ConnectionString ?? null;
        }
        protected string GetBusinessGroupId()
        {
            // pre processing
            var simplePrinciple = (ClaimsPrincipal)HttpContext.Current.User;
            var identity = simplePrinciple?.Identity as ClaimsIdentity;
            return identity?.FindFirst("BusinessGroupId")?.Value;
        }

        protected string GETRIN()
        {
            // pre processing
            var simplePrinciple = (ClaimsPrincipal)HttpContext.Current.User;
            var identity = simplePrinciple?.Identity as ClaimsIdentity;
            return identity?.FindFirst("RIN")?.Value;
        }
    }
}