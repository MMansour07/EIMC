using eInvoicing.DTO;
using eInvoicing.Web.Helper;
using eInvoicing.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using eInvoicing.Service.Helper.Extension;
using System.Security.Claims;
using eInvoicing.Web.Filters;
using System.Net;
using System.Net.Http;
using System.Diagnostics;

namespace eInvoicing.Web.Controllers
{
    [Authorize]
    public class CodeController : Controller
    {
        private readonly IUserSession _userSession;
        private readonly IHttpClientHandler _httpClient;
        public CodeController(IHttpClientHandler httpClient, IUserSession userSession)
        {
            _httpClient = httpClient;
            _userSession = userSession;
        }
        
        [HttpGet]
        [ActionName("codeslist")]
        public ActionResult SearchMyEGSCodeUsageRequests()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ajax_SearchMyEGSCodeUsageRequests")]
        public ActionResult ajaxSearchMyEGSCodeUsageRequests()
        {
            try
            {
                var pageNumber = Request["pagination[page]"];
                var pageSize = Request["pagination[perpage]"];
                string fromDate = Request["fromDate"];
                string toDate = Request["toDate"];
                var sortDirection = Request["sort[sort]"];
                var status = Request["query[status]"];
                var firstDayOfMonth = DateTime.Now.AddYears(-2);
                var Today = DateTime.Now;
                //DateTime _fromDate = string.IsNullOrEmpty(fromDate) ? firstDayOfMonth : DateTime.ParseExact(fromDate, "yyyy-MM-ddThh:mm:ssZ", CultureInfo.InvariantCulture);
                //DateTime _toDate = string.IsNullOrEmpty(toDate) ? Today : DateTime.ParseExact(toDate, "yyyy-MM-ddThh:mm:ssZ", CultureInfo.InvariantCulture);
                var req = new SearchEGSCodeRequestDTO()
                {
                    pageNumber = Convert.ToInt32(pageNumber),
                    active = true,
                    ActiveFrom = DateTime.UtcNow.AddYears(-2),
                    ActiveTo = DateTime.UtcNow,
                    CodeName = null,
                    ItemCode = null,
                    orderDirections = sortDirection,
                    CodeDescription = null,
                    pageSize = Convert.ToInt32(pageSize),
                    status = status,
                    ParentItemCode = null,
                    ParentLevelName = null
                };
                string url = "api/code/SearchMyEGSCodeUsageRequests/";
                var response = _httpClient.POST(url, req);
                var Content = JsonConvert.DeserializeObject<SearchEGSCodeResponseDTO>(response.Info);
                return Json(new SearchEGSCodeResponse()
                {
                    data = PagedList<SearchEGSCodeResultDTO>.Create(Content?.result, Convert.ToInt32(pageNumber), Convert.ToInt32(pageSize), Content?.metadata?.totalCount),
                    meta = new Meta()
                    {
                        pages = Content?.metadata?.totalPages,
                        total = Content?.metadata?.totalCount
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new genericResponse() { Message = "Calling Preparation error! --> [" + ex.Message.ToString() + "]" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
