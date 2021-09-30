using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation;
using eInvoicing.Service.Helper;
using eInvoicing.Web.Helper;
using eInvoicing.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eInvoicing.Web.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IUserSession _userSession;
        public ReportController(IUserSession userSession)
        {
            _userSession = userSession;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult documents_stats()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult top_goods_usage()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult monthly_bestseller()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult monthly_lowestseller()
        {
            return View();
        }

        [HttpPost]
        [ActionName("AjaxGetSubmittedDocumentsStats")]
        public ActionResult AjaxGetSubmittedDocumentsStats()
        {
            try
            {
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string fromDate = Request["fromDate"];
                string toDate = Request["toDate"];
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var Today = DateTime.Now;
                DateTime _fromDate = string.IsNullOrEmpty(fromDate) ? firstDayOfMonth : Convert.ToDateTime(fromDate);
                DateTime _toDate = string.IsNullOrEmpty(toDate) ? Today : Convert.ToDateTime(toDate);
                if (start == 0)
                    start = 1;
                else
                    start = (start / length)+1;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    var url = _userSession.URL + "api/report/submitteddocumentsstats?pageNumber="+Convert.ToInt32(start)+"&pageSize="+ Convert.ToInt32(length)+"&fromdate="+ _fromDate +"&todate="+ _toDate + "&searchValue="+searchValue+ "&sortColumnName="+sortColumnName+ "&sortDirection="+sortDirection;
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<SubmittedDocumentResponse>(postTask.Content.ReadAsStringAsync().Result);
                        return Json (new {recordsTotal = response.meta.total, recordsFiltered = response.meta.totalFiltered,  data = response.data }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new SubmittedDocumentResponse(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new SubmittedDocumentResponse(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("AjaxTopGoodsUsage")]
        public ActionResult AjaxTopGoodsUsage()
        {
            try
            {
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string fromDate = Request["fromDate"];
                string toDate = Request["toDate"];
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var Today = DateTime.Now;
                DateTime _fromDate = string.IsNullOrEmpty(fromDate) ? firstDayOfMonth : Convert.ToDateTime(fromDate);
                DateTime _toDate = string.IsNullOrEmpty(toDate) ? Today : Convert.ToDateTime(toDate);
                if (start == 0)
                    start = 1;
                else
                    start = (start / length) + 1;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    var url = _userSession.URL + "api/report/TopGoodsUsage?pageNumber=" + Convert.ToInt32(start) + 
                        "&pageSize=" + Convert.ToInt32(length) + "&fromdate=" + _fromDate + "&todate=" + _toDate + "&searchValue=" + searchValue + "&sortColumnName=" + sortColumnName + "&sortDirection=" + sortDirection;
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<GoodsModelVM>(postTask.Content.ReadAsStringAsync().Result);
                        return Json(new { recordsTotal = response.meta.total, recordsFiltered = response.meta.totalFiltered, data = response.data }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new GoodsModelVM(), JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new GoodsModelVM(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("AjaxgetMonthlyBestSeller")]
        public ActionResult AjaxgetMonthlyBestSeller(DatatableInputParam obj)
        {
            try
            {
                if (obj.start == 0)
                    obj.start = 1;
                else
                    obj.start = (obj.start / obj.length) + 1;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    var url = _userSession.URL + "api/report/MonthlyBestSeller?SpecificDate=" + obj.specificDate;
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<List<GoodsModel>>(postTask.Content.ReadAsStringAsync().Result);
                        return Json(new { recordsTotal = response.Count(), recordsFiltered = response.Count(), data = response }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new List<GoodsModel>(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new List<GoodsModel>(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("AjaxgetMonthlyLowestSeller")]
        public ActionResult AjaxgetMonthlyLowestSeller(DatatableInputParam obj)
        {
            try
            {
                if (obj.start == 0)
                    obj.start = 1;
                else
                    obj.start = (obj.start / obj.length) + 1;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    var url = _userSession.URL + "api/report/MonthlyLowestSeller?SpecificDate=" + obj.specificDate;
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<List<GoodsModel>>(postTask.Content.ReadAsStringAsync().Result);
                        return Json(new {recordsTotal =response.Count(), recordsFiltered = response.Count(), data = response}, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new List<GoodsModel>(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new List<GoodsModel>(), JsonRequestBehavior.AllowGet);
            }
        }

    }
}
