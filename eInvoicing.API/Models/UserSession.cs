using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace eInvoicing.API.Models
{
    public class UserSession : IUserSession
    {
        public UserSession()
        {
            if (ConfigurationManager.AppSettings["Environment"].ToLower() == "preprod")
            {
                url = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                submissionurl = ConfigurationManager.AppSettings["apiBaseUrl"];
                loginUrl = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                client_id = ConfigurationManager.AppSettings["client_id"];
                client_secret = ConfigurationManager.AppSettings["client_secret"];
            }
            else
            {
                url = ConfigurationManager.AppSettings["ProdidSrvBaseUrl"];
                submissionurl = ConfigurationManager.AppSettings["ProdapiBaseUrl"];
                loginUrl = ConfigurationManager.AppSettings["ProdidSrvBaseUrl"];
                client_id = ConfigurationManager.AppSettings["Prod_client_id"];
                client_secret = ConfigurationManager.AppSettings["Prod_client_secret"];
            }
            submitServiceUrl = ConfigurationManager.AppSettings["submitSrvBaseUrl"];
        }
        public string url { get ; set ; }
        public string loginUrl { get ; set ; }
        public string submitServiceUrl { get ; set ; }
        public string submissionurl { get ; set ; }
        public string client_id { get ; set ; }
        public string client_secret { get ; set ; }
    }
}