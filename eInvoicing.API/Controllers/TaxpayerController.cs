using eInvoicing.API.Filters;
using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public string token(string clientId = null)
        {
            try
            {
                return _taxpayerService.token()?.Replace("\r\n", "");
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
                return _taxpayerService.GetClientId()?.Replace("\r\n", "");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/taxpayer/details")]
        public IHttpActionResult GetTaxPayerDetails(string clientId = null)
        {
            try
            {
                return Ok(_taxpayerService.getTaxpayerDetails());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
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