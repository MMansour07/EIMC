using System.Configuration;
using eInvoicing.Service.AppService.Contract.Base;

namespace eInvoicing.API.Models
{
    public class UserSession : IUserSession
    {
        private readonly ITaxpayerService _taxpayerService;
        public UserSession(ITaxpayerService taxpayerService)
        {
            _taxpayerService = taxpayerService;
            var taxpayer = _taxpayerService.getTaxpayerDetails();
            if (ConfigurationManager.AppSettings["Environment"].ToLower() == "preprod")
            {
                url = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                submissionurl = ConfigurationManager.AppSettings["apiBaseUrl"];
                loginUrl = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                client_id = taxpayer.PreProdClientId; //ConfigurationManager.AppSettings["client_id"];
                client_secret = taxpayer.PreProdClientSecret; //ConfigurationManager.AppSettings["client_secret"];
            }
            else
            {
                url = ConfigurationManager.AppSettings["ProdidSrvBaseUrl"];
                submissionurl = ConfigurationManager.AppSettings["ProdapiBaseUrl"];
                loginUrl = ConfigurationManager.AppSettings["ProdidSrvBaseUrl"];
                client_id = taxpayer.ProdClientId; //ConfigurationManager.AppSettings["Prod_client_id"];
                client_secret = taxpayer.ProdClientSecret; //ConfigurationManager.AppSettings["Prod_client_secret"];
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