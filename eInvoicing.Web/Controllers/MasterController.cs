﻿using eInvoicing.DTO;
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
    }
}