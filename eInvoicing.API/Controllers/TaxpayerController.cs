using eInvoicing.API.Filters;
using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace eInvoicing.API.Controllers
{
    public class TaxpayerController : ApiController
    {
        private readonly ITaxpayerService _taxpayerService;
        public TaxpayerController(ITaxpayerService taxpayerService)
        {
            _taxpayerService = taxpayerService;
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("api/taxpayer")]
        public IHttpActionResult add(TaxpayerDTO obj)
        {
            try
            {
                _taxpayerService.add(obj);
                return Ok(new {sucess = true});
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/taxpayer/GetLicense")]
        public string token()
        {
            try
            {
                Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
                if (objAppsettings.Settings["IsExternal"]?.Value == "1")
                {
                    return _taxpayerService.TokenByBusinessGroup(objAppsettings.Settings["Current_BusinessGroup"]?.Value)?.Replace("\r\n", "");
                }
                else
                {
                    return _taxpayerService.TokenByBusinessGroupId(objAppsettings.Settings["Current_BusinessGroupId"]?.Value?.Split('&')?[0]?.Replace("\r\n", ""))?.Replace("\r\n", "");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        [Route("api/taxpayer/GetEncryptionKey")]
        public string GetEncryptionKey()
        {
            try
            {
                Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
                if (objAppsettings.Settings["IsExternal"]?.Value == "1")
                {
                    return _taxpayerService.GetClientIdByBusinessGroup(objAppsettings.Settings["Current_BusinessGroup"]?.Value)?.Replace("\r\n", "");
                }
                else
                {
                    return _taxpayerService.GetClientIdByBusinessGroupId(objAppsettings.Settings["Current_BusinessGroupId"]?.Value?.Split('&')?[0]?.Replace("\r\n", ""))?.Replace("\r\n", "");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/taxpayer/details")]
        public IHttpActionResult GetTaxPayerDetails(string BusinessGroupId)
        {
            try
            {
                return Ok(_taxpayerService.getTaxpayerDetails(BusinessGroupId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/taxpayer/GetDate")]
        public string GetDate()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new Uri("http://worldclockapi.com/api/json/est/now");
                    var postTask = client.GetAsync("http://worldclockapi.com/api/json/est/now");
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<OnlineDateDTO>(result.Content.ReadAsStringAsync().Result);
                        return response.currentDateTime.Split('T')[0];
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}