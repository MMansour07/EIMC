using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using eInvoicing.API.Filters;
using eInvoicing.API.Models;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.Helper;
using eInvoicing.API.Helper;
using Newtonsoft.Json;
using eInvoicing.DTO;

namespace eInvoicing.API.Controllers
{
    [JwtAuthentication]
    public class AppSettingController : ApiController
    {
        private readonly string url = ConfigurationManager.AppSettings["idSrvBaseUrl"];
        private readonly string submitServiceUrl = ConfigurationManager.AppSettings["submitSrvBaseUrl"];
        private readonly string submissionurl = ConfigurationManager.AppSettings["apiBaseUrl"];
        private readonly string client_id = ConfigurationManager.AppSettings["client_id"];
        private readonly string client_secret = ConfigurationManager.AppSettings["client_secret"];
        private readonly ITaxpayerService _taxpayerService;

        public AppSettingController(ITaxpayerService taxpayerService)
        {
            _taxpayerService = taxpayerService;
        }

        [HttpGet, ActionName("GetChannelManagerSettings")]
        public IHttpActionResult Settings()
        {
            try
            {
                return Ok(new SettingViewModel()
                {
                    PreProductionLoginURL = ConfigurationManager.AppSettings["idSrvBaseUrl"],
                    PreProductionApiURL = ConfigurationManager.AppSettings["apiBaseUrl"],
                    PreProductionAppId = ConfigurationManager.AppSettings["AppId"],
                    PreProductionClientSecret = ConfigurationManager.AppSettings["client_secret"],
                    PreProductionClientId = ConfigurationManager.AppSettings["client_id"],
                    PreProductionAppKey = ConfigurationManager.AppSettings["AppKey"],
                    DevSignerURL = ConfigurationManager.AppSettings["submitSrvBaseUrl"],
                    ProductionAppKey = ConfigurationManager.AppSettings["ProdAppKey"],
                    ProductionAppId = ConfigurationManager.AppSettings["ProdAppId"],
                    ProductionClientId = ConfigurationManager.AppSettings["Prod_client_id"],
                    ProductionClientSecret = ConfigurationManager.AppSettings["Prod_client_secret"],
                    ProductionSignerURL = ConfigurationManager.AppSettings["ProdsubmitSrvBaseUrl"],
                    ProductionLoginURL = ConfigurationManager.AppSettings["ProdidSrvBaseUrl"],
                    ProductionApiURL = ConfigurationManager.AppSettings["ProdapiBaseUrl"],
                    APIsEnvironment = ConfigurationManager.AppSettings["Environment"].ToLower() == "preprod" ? false : true,
                    TypeVersion = ConfigurationManager.AppSettings["TypeVersion"].ToLower() == "0.9" ? false : true
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, ActionName("ConfigureChannelManagerSettings")]
        public IHttpActionResult Update(SettingViewModel model)
        {
            try
            {
                if (model.APIsEnvironment)
                {
                    _taxpayerService.updateTaxPayer(new TaxpayerDTO() { ClientId = model.ProductionClientId, ClientSecret = model.ProductionClientSecret });
                }
                else
                {
                    _taxpayerService.updateTaxPayer(new TaxpayerDTO() { ClientId = model.PreProductionClientId, ClientSecret = model.PreProductionClientSecret });
                }
                Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
                var connectionStringsSection = (ConnectionStringsSection)objConfig.GetSection("connectionStrings");
                //Edit
                if (objAppsettings != null)
                {
                    objAppsettings.Settings["apiBaseUrl"].Value = model.PreProductionApiURL;
                    objAppsettings.Settings["idSrvBaseUrl"].Value = model.PreProductionLoginURL;
                    //objAppsettings.Settings["client_id"].Value = model.PreProductionClientId;
                    //objAppsettings.Settings["client_secret"].Value = model.PreProductionClientSecret;
                    //objAppsettings.Settings["Prod_client_id"].Value = model.ProductionClientId;
                    //objAppsettings.Settings["Prod_client_secret"].Value = model.ProductionClientSecret;
                    objAppsettings.Settings["ProdapiBaseUrl"].Value = model.ProductionApiURL;
                    objAppsettings.Settings["ProdidSrvBaseUrl"].Value = model.ProductionLoginURL;
                    objAppsettings.Settings["Environment"].Value = model.APIsEnvironment ? "Prod" : "PreProd";
                    objAppsettings.Settings["TypeVersion"].Value = model.TypeVersion ? "1.0" : "0.9";
                    if (model.APIsEnvironment)
                    {
                        connectionStringsSection.ConnectionStrings["eInvoicing_CS"].ConnectionString = objAppsettings.Settings["ConnectioString_Prod"].Value;
                    }
                    else
                    {
                        connectionStringsSection.ConnectionStrings["eInvoicing_CS"].ConnectionString = objAppsettings.Settings["ConnectioString_PreProd"].Value;
                    }
                    objConfig.Save();
                    return Ok();
                }
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }

    }
}
