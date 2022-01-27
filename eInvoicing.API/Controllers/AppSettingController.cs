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
using System.Security.Claims;

namespace eInvoicing.API.Controllers
{
    [JwtAuthentication]
    public class AppSettingController : ApiController
    {
        private readonly ITaxpayerService _taxpayerService;

        public AppSettingController(ITaxpayerService taxpayerService)
        {
            _taxpayerService = taxpayerService;
        }

        [HttpGet, ActionName("GetChannelManagerSettings")]
        public IHttpActionResult Settings(string BusinessGroupId)
        {
            try
            {

                var taxpayer = _taxpayerService.getTaxpayerDetails(BusinessGroupId);
                return Ok(new SettingViewModel()
                {
                    PreProductionLoginURL = ConfigurationManager.AppSettings["idSrvBaseUrl"],
                    PreProductionApiURL = ConfigurationManager.AppSettings["apiBaseUrl"],
                    PreProductionAppId = ConfigurationManager.AppSettings["AppId"],
                    PreProductionClientSecret = taxpayer.PreProdClientSecret,
                    PreProductionClientId = taxpayer.PreProdClientId,
                    PreProductionAppKey = ConfigurationManager.AppSettings["AppKey"],
                    DevSignerURL = ConfigurationManager.AppSettings["submitSrvBaseUrl"],
                    ProductionAppKey = ConfigurationManager.AppSettings["ProdAppKey"],
                    ProductionAppId = ConfigurationManager.AppSettings["ProdAppId"],
                    ProductionClientId = taxpayer.ProdClientId,
                    ProductionClientSecret = taxpayer.ProdClientSecret,
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
                _taxpayerService.updateTaxPayer(new TaxpayerDTO()
                {
                    PreProdClientId = model.PreProductionClientId,
                    PreProdClientSecret = model.PreProductionClientSecret,
                    ProdClientSecret = model.ProductionClientSecret,
                    ProdClientId = model.ProductionClientId
                });
                Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
                var connectionStringsSection = (ConnectionStringsSection)objConfig.GetSection("connectionStrings");
                //Edit
                if (objAppsettings != null)
                {
                    objAppsettings.Settings["apiBaseUrl"].Value = model.PreProductionApiURL;
                    objAppsettings.Settings["idSrvBaseUrl"].Value = model.PreProductionLoginURL;
                    objAppsettings.Settings["ProdapiBaseUrl"].Value = model.ProductionApiURL;
                    objAppsettings.Settings["ProdidSrvBaseUrl"].Value = model.ProductionLoginURL;
                    objAppsettings.Settings["Environment"].Value = model.APIsEnvironment ? "Prod" : "PreProd";
                    objAppsettings.Settings["TypeVersion"].Value = model.TypeVersion ? "1.0" : "0.9";


                    var simplePrinciple = (ClaimsPrincipal)HttpContext.Current.User;
                    var identity = simplePrinciple?.Identity as ClaimsIdentity;

                    string connString = ConfigurationManager.ConnectionStrings["EInvoice_" + identity?.FindFirst("BusinessGroup")?.Value?.Replace(" ", "")]?.ConnectionString;
                    string preprod_connString = "";
                    string Temp = connString?.Split(';')[1]?.Split('=')[1];
                    if (!Temp.ToLower().Contains("preprod"))
                        preprod_connString = connString.Replace(identity?.FindFirst("BusinessGroup").Value.Replace(" ", ""), identity?.FindFirst("BusinessGroup").Value.Replace(" ", "") + "_PreProd");
                    else
                        preprod_connString = connString.Replace(identity?.FindFirst("BusinessGroup").Value.Replace(" ", ""), identity?.FindFirst("BusinessGroup").Value.Replace(" ", ""));

                    string prod_connString = connString.Replace(identity?.FindFirst("BusinessGroup").Value.Replace(" ", "") + "_PreProd", identity?.FindFirst("BusinessGroup").Value.Replace(" ", ""));

                    if (model.APIsEnvironment)
                    {
                        connectionStringsSection.ConnectionStrings["EInvoice_" + identity?.FindFirst("BusinessGroup").Value.Replace(" ", "")].ConnectionString = prod_connString;
                    }
                    else
                    {
                        connectionStringsSection.ConnectionStrings["EInvoice_" + identity?.FindFirst("BusinessGroup").Value.Replace(" ", "")].ConnectionString = preprod_connString;
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
