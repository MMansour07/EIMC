using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using eInvoicing.Web.Helper;
using eInvoicing.Web.Models;
using Newtonsoft.Json;

namespace eInvoicing.Web.Controllers
{
    [Authorize]
    public class AppSettingController : Controller
    {
        private readonly IUserSession _userSession;
        private readonly IHttpClientHandler _httpClient;
        public AppSettingController(IHttpClientHandler httpClient, IUserSession userSession)
        {
            _httpClient = httpClient;
            _userSession = userSession;
        }
        public ActionResult Index()
        {
            try
            {
                var response = JsonConvert.DeserializeObject<SettingViewModel>(_httpClient.GET("api/appsetting/GetChannelManagerSettings").Info);
                if (response != null)
                {
                    response.DevAPIURL = ConfigurationManager.AppSettings["DevSrvBaseUrl"];
                    response.ProductionInternalAPIURL = ConfigurationManager.AppSettings["ProdSrvBaseUrl"];
                    response.Environment = ConfigurationManager.AppSettings["Environment"].ToLower() == "development" ? false : true;
                    return View(response);
                }
                return View(new SettingViewModel()
                {
                    DevAPIURL = ConfigurationManager.AppSettings["DevSrvBaseUrl"],
                    ProductionInternalAPIURL = ConfigurationManager.AppSettings["ProdSrvBaseUrl"],
                    Environment = ConfigurationManager.AppSettings["Environment"].ToLower() == "development" ? false : true
                });
            }
            catch
            {
                return View(new SettingViewModel()
                {
                    DevAPIURL = ConfigurationManager.AppSettings["DevSrvBaseUrl"],
                    ProductionInternalAPIURL = ConfigurationManager.AppSettings["ProdSrvBaseUrl"],
                    Environment = ConfigurationManager.AppSettings["Environment"].ToLower() == "development" ? false : true
                });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(SettingViewModel model)
        {
            try
            {
                var response = _httpClient.POST("api/appsetting/ConfigureChannelManagerSettings", model);
                if (response.Message.ToLower() == "success")
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}