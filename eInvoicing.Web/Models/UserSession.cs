using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace eInvoicing.Web.Models
{
    public class UserSession : IUserSession
    {
        public string Username
        {
            get { return ((ClaimsPrincipal)HttpContext.Current.User)?.FindFirst(ClaimTypes.Name)?.Value; }
        }

        public string BearerToken
        {
            get { return ((ClaimsPrincipal)HttpContext.Current.User)?.FindFirst("AcessToken")?.Value; }
        }
        public string URL
        {
            get
            {
                return ConfigurationManager.AppSettings["Environment"].ToLower() == "development" ?
                ConfigurationManager.AppSettings["DevSrvBaseUrl"] : ConfigurationManager.AppSettings["ProdSrvBaseUrl"];
            }
        }


    }
}