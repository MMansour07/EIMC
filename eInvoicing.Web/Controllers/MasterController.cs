using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation;
using eInvoicing.Web.Filters;
using eInvoicing.Web.Helper;
using eInvoicing.Web.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace eInvoicing.Web.Controllers
{
    [Authorize]
    public class MasterController : Controller
    {
        private readonly IUserSession _userSession;
        private readonly IHttpClientHandler _httpClient;
        Logger logger = LogManager.GetCurrentClassLogger();

        public MasterController(IHttpClientHandler httpClient, IUserSession userSession)
        {
            _httpClient = httpClient;
            _userSession = userSession;
        }
        public ActionResult Index()
        {
            if(Request.Cookies["Language"] == null)
            {
                HttpCookie cookie = new HttpCookie("Language");
                cookie.Value = "en";
                Response.Cookies.Add(cookie);
            }
            return View();
        }
        public ActionResult ChangeLanguage(string language)
        {
            Request.Cookies["Language"].Value = language;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Response.Cookies.Add(Request.Cookies["Language"]);
            return View("Index");
        }
        [HttpGet]
        [ActionName("renderer")]
        public ActionResult Renderer(string _date)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    var url = _userSession.URL + "api/document/MonthlyDocuments?_date=" + DateTime.ParseExact(_date, "yyyy-MM-dd",CultureInfo.InvariantCulture);
                    
                    logger.Info(url);
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<DashboardDTO>(postTask.Content.ReadAsStringAsync().Result);
                        if (string.IsNullOrEmpty(response.Reason))
                            return Json(new { status = "Success", data = response }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = postTask.StatusCode, ReasonPhrase = postTask.ReasonPhrase, RequestMessage = postTask.RequestMessage, Content = postTask.Content.ReadAsStringAsync()}, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(new { status = ex.Message.ToString()}, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [ActionName("DraftCount")]
        public ActionResult Draft()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    var url = _userSession.URL + "api/document/pendingCount";
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        int docsCount = JsonConvert.DeserializeObject<int>(postTask.Content.ReadAsStringAsync().Result);
                        var _response = new DashboardDTO()
                        {
                            allDraftedDocumentsCount = docsCount
                        };
                        return Json(_response, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = "Failed", Message = postTask.StatusCode}, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = ex.Message.ToString(), Message = 500 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [ActionName("SentCount")]
        public ActionResult Sent()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    var url = _userSession.URL + "api/document/submittedCount";
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        int docsCount = JsonConvert.DeserializeObject<int>(postTask.Content.ReadAsStringAsync().Result);
                        var _response = new DashboardDTO()
                        {
                            allSubmittedDocumentsCount = docsCount
                        };
                        return Json(_response, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = "Failed", Message = postTask.StatusCode }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { status = "Failed", Message = 500 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ActionName("receivedCount")]
        public ActionResult Received()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    var url = _userSession.URL + "api/document/receivedCount";
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        int docsCount = JsonConvert.DeserializeObject<int>(postTask.Content.ReadAsStringAsync().Result);
                        var _response = new DashboardDTO()
                        {
                            ReceivedDocumentsCount = docsCount
                        };
                        return Json(_response, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = "Failed", Message = postTask.StatusCode }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { status = "Failed", Message = 500 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [ActionName("InvalidandFailedCount")]
        public ActionResult InvalidandFailedCount()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    var url = _userSession.URL + "api/document/InvalidandFailedCount";
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        int docsCount = JsonConvert.DeserializeObject<int>(postTask.Content.ReadAsStringAsync().Result);
                        var _response = new DashboardDTO()
                        {
                            InvalidandFailedCount = docsCount
                        };
                        return Json(_response, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = "Failed", Message = postTask.StatusCode }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { status = "Failed", Message = 500 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ActionName("SyncCustomerDocumentsByCurrentloggedinOrg")]
        public ActionResult SyncCustomerDocumentsByCurrentloggedinOrg()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    client.Timeout = TimeSpan.FromMinutes(60);
                    var url = _userSession.URL + "/api/document/SyncCustomerDocumentsByCurrentloggedinOrg";
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else if (postTask.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return Json(new { success = false, message = "400" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}