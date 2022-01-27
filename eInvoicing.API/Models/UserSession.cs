using System.Configuration;
using System.Security.Claims;
using System.Web;
using eInvoicing.Service.AppService.Contract.Base;

namespace eInvoicing.API.Models
{
    public class UserSession : IUserSession
    {
        private readonly ITaxpayerService _taxpayerService;
        public string BGId { get; set; }
        public string BusinessGroupName { get; set; }
        public void GetBusinessGroupId(string BusinessGroupId)
        {
            BGId = BusinessGroupId;
            var taxpayer = _taxpayerService.getTaxpayerDetails(BGId);
            if (ConfigurationManager.AppSettings["Environment"].ToLower() == "preprod")
            {
                url = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                submissionurl = ConfigurationManager.AppSettings["apiBaseUrl"];
                loginUrl = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                InternalAPIURL = ConfigurationManager.AppSettings["InternalAPIURL_IISExpress"];
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
                InternalAPIURL = ConfigurationManager.AppSettings["InternalAPIURL_IIS"];
            }
            pin = taxpayer.PIN;
            submitServiceUrl = ConfigurationManager.AppSettings["submitSrvBaseUrl"];
        }
        public void SetBusinessGroup(string BusinessGroup)
        {
            BusinessGroupName = BusinessGroup.Replace("E Invoice _ ", "");
            var taxpayer = _taxpayerService.getTaxpayerDetailsByBusinessGroupName(BusinessGroupName);
            if (ConfigurationManager.AppSettings["Environment"].ToLower() == "preprod")
            {
                url = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                submissionurl = ConfigurationManager.AppSettings["apiBaseUrl"];
                loginUrl = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                InternalAPIURL = ConfigurationManager.AppSettings["InternalAPIURL_IISExpress"];
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
                InternalAPIURL = ConfigurationManager.AppSettings["InternalAPIURL_IIS"];
            }
            pin = taxpayer.PIN;
            submitServiceUrl = ConfigurationManager.AppSettings["submitSrvBaseUrl"];
        }

        public bool IsDBSync(string BusinessGroup)
        {
            BusinessGroupName = BusinessGroup.Replace("E Invoice _ ", "");
            return _taxpayerService.CheckIsDBSync(BusinessGroupName);
        }
        public UserSession(ITaxpayerService taxpayerService)
        {
            _taxpayerService = taxpayerService;
            var taxpayer = _taxpayerService.getTaxpayerDetails(BGId);
            if (ConfigurationManager.AppSettings["Environment"].ToLower() == "preprod")
            {
                url = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                submissionurl = ConfigurationManager.AppSettings["apiBaseUrl"];
                loginUrl = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                InternalAPIURL = ConfigurationManager.AppSettings["InternalAPIURL_IISExpress"];
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
                InternalAPIURL = ConfigurationManager.AppSettings["InternalAPIURL_IIS"];
            }
            submitServiceUrl = ConfigurationManager.AppSettings["submitSrvBaseUrl"];
        }
        public string url { get ; set ; }
        public string loginUrl { get ; set ; }
        public string InternalAPIURL { get ; set ; }
        public string submitServiceUrl { get ; set ; }
        public string submissionurl { get ; set ; }
        public string pin { get ; set ; }
        public string client_id { get ; set ; }
        public string client_secret { get ; set ; }
    }
}