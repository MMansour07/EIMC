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
                var req = new SearchEGSCodeRequestDTO()
                {
                    pageNumber = Convert.ToInt32(Request["pagination[page]"]),
                    pageSize = Convert.ToInt32(Request["pagination[perpage]"]),
                    orderDirections = Request["sort[sort]"],
                    active = Request["active"],
                    activeFrom = Request["fromDate"],
                    activeTo = Request["toDate"],
                    codeName = Request["codeName"],
                    itemCode = Request["itemCode"],
                    codeDescription = Request["codeDescription"],
                    status = Request["status"]
                };
                string url = "api/code/SearchMyEGSCodeUsageRequests/";
                var response = _httpClient.POST(url, req);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var Content = JsonConvert.DeserializeObject<SearchEGSCodeResponseDTO>(response.Info);
                    return Json(new SearchEGSCodeResponse()
                    {
                        data = PagedList<SearchEGSCodeResultDTO>.Create(Content?.result, Convert.ToInt32(Request["pagination[page]"]), Convert.ToInt32(Request["pagination[perpage]"]), Content?.metadata?.totalCount),
                        meta = new Meta()
                        {
                            page = Convert.ToInt32(Request["pagination[page]"]),
                            perpage = Convert.ToInt32(Request["pagination[perpage]"]),
                            pages = Content?.metadata?.totalPages,
                            total = Content?.metadata?.totalCount
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new genericResponse() { Message = response.HttpResponseMessage.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new genericResponse() { Message = "Internal Error! --> [" + ex.Message.ToString() + "]" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("SearchPublishedCodes")]
        public ActionResult SearchPublishedCodes()
        {
            try
            {
                var req = new SearchPublishedCodesRequestDTO()
                {
                    pn = Convert.ToInt32(Request["pagination[page]"]),
                    ps = Convert.ToInt32(Request["pagination[perpage]"]),
                    codeName = Request["codeName"],
                    codeLookupValue = Request["itemCode"],
                    codeType = Request["CodeType"],
                };
                string url = "api/code/SearchPublishedCodes/";
                var response = _httpClient.POST(url, req);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var Content = JsonConvert.DeserializeObject<SearchPublishedCodesResponseDTO>(response.Info);
                    return Json(new SearchEGSPublishedCodeResponse()
                    {
                        data = PagedList<SearchPublishedCodesResultDTO>.Create(Content?.result, Convert.ToInt32(Request["pagination[page]"]),
                        Convert.ToInt32(Request["pagination[perpage]"]), Content?.metadata?.totalCount),
                        meta = new Meta()
                        {
                            page = Convert.ToInt32(Request["pagination[page]"]),
                            perpage = Convert.ToInt32(Request["pagination[perpage]"]),
                            pages = Content?.metadata?.totalPages,
                            total = Content?.metadata?.totalCount
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new genericResponse() { Message = response.HttpResponseMessage.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new genericResponse() { Message = "Internal Error! --> [" + ex.Message.ToString() + "]" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ActionName("SearchPublishedCodesByKey")]
        public ActionResult SearchPublishedCodesBykey(string searchKey,  string searchValue, string codeType)
        {
            try
            {
                var req = new SearchPublishedCodesRequestDTO()
                {
                    pn = 1,
                    ps = 10,
                    codeName = searchKey == "CodeName" ? searchValue : "",
                    codeLookupValue = searchKey != "CodeName" ? searchValue : "",
                    codeType = codeType,
                };
                string url = "api/code/SearchPublishedCodes/";
                var response = _httpClient.POST(url, req);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var Content = JsonConvert.DeserializeObject<SearchPublishedCodesResponseDTO>(response.Info);
                    return Json(Content?.result, JsonRequestBehavior.AllowGet);
                }
                return Json(new genericResponse() { Message = response.HttpResponseMessage.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new genericResponse() { Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
