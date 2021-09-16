using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation;
using eInvoicing.Web.Filters;
using eInvoicing.Web.Helper;
using eInvoicing.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public MasterController(IHttpClientHandler httpClient, IUserSession userSession)
        {
            _httpClient = httpClient;
            _userSession = userSession;
        }
        public ActionResult Index()
        {
            return View();
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
                    var url = _userSession.URL + "api/document/MonthlyDocuments?_date=" + DateTime.Parse(_date.ToString());
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<DashboardDTO>(postTask.Content.ReadAsStringAsync().Result);
                        return Json(new { status = "Success", data = response}, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = "Failed"}, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { status = "Failed"}, JsonRequestBehavior.AllowGet);
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
            catch
            {
                return Json(new { status = "Failed", Message = 500 }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //[ActionName("UploadLicense")]
        //public ActionResult UploadLicense()
        //{

        //    if (Request.Files["License"].ContentLength > 0)
        //    {
        //        LicenseDTO License = new LicenseDTO();
        //        var fullPath = Server.MapPath("~/Content/License/" + Request.Files["License"].FileName);
        //        Request.Files["License"].SaveAs(fullPath);
        //        using (StreamReader r = new StreamReader(fullPath))
        //        {
        //            string json = r.ReadToEnd();
        //            License = JsonConvert.DeserializeObject<LicenseDTO>(json);
        //        }
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Clear();
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
        //            var url = _userSession.URL + "api/lookup/InsertLicenseValue";
        //            client.BaseAddress = new Uri(url);
        //            var postTask = Task.Run(() => client.PostAsJsonAsync(url, License)).Result;
        //            if (postTask.IsSuccessStatusCode)
        //            {
        //                return Json("File imported successfully");
        //            }
        //        }
        //    }

        //    return Json("File is Empty");
        //}

        [HttpPost]
        [ActionName("UploadLicense")]
        public ActionResult UploadLicense()
        {
            LicenseDTO License = new LicenseDTO();
            try
            {
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }
                    // Get the complete folder path and store the file inside it.  
                    fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), fname);
                    file.SaveAs(fname);
                    using (StreamReader r = new StreamReader(fname))
                    {
                        string json = r.ReadToEnd();
                        License = JsonConvert.DeserializeObject<LicenseDTO>(json);
                    }
                    var response = _httpClient.POST("api/lookup/InsertLicenseValue", License);
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return Json(new {success = true}, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}