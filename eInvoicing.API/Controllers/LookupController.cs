using eInvoicing.API.Filters;
using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using Newtonsoft.Json;
using ProductLicense;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace eInvoicing.API.Controllers
{
    [JwtAuthentication]
    public class LookupController : ApiController
    {
        private readonly ILookupService _lookupService;
        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }
        [HttpPost]
        [Route("api/lookup/InsertLicenseValue")]
        public IHttpActionResult InsertEncryptedLicenseValue(LicenseDTO License)
        {
            try
            {
                //var loggedInuser = User.Identity.Name;
                _lookupService.InsertLicense(License.License, User.Identity.Name);
                return Ok(new {sucess = true});
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/lookup/GetLicense")]
        public string GetEncryptedLicenseValue(string Client_Id)
        {
            try
            {
               return _lookupService.GetEncryptedLicenseValue("License").Replace("\r\n", "");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        [Route("api/lookup/GetDate")]
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