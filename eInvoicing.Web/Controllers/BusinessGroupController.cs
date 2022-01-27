using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using eInvoicing.DTO;
using eInvoicing.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace eInvoicing.Web.Controllers
{
    [Authorize]

    public class BusinessGroupController : Controller
    {
        private readonly IUserSession _userSession;
        public BusinessGroupController(IUserSession userSession)
        {
            _userSession = userSession;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetBusinessGroups")]
        public ActionResult GetBusinessGroups()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                        client.Timeout = TimeSpan.FromMinutes(60);
                        var url = _userSession.URL + "/api/BG/getgroups";
                        client.BaseAddress = new Uri(url);
                        var postTask = Task.Run(() => client.GetAsync(url)).Result;
                        if (postTask.IsSuccessStatusCode)
                        {
                            var users = JsonConvert.DeserializeObject<List<BusinessGroupDTO>>(postTask.Content.ReadAsStringAsync().Result);
                            return Json(users, JsonRequestBehavior.AllowGet);
                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateGroup")]
        public ActionResult Create(BusinessGroupDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                        client.Timeout = TimeSpan.FromMinutes(60);
                        var url = _userSession.URL + "/api/BG/create";
                        client.BaseAddress = new Uri(url);
                        var postTask = Task.Run(() => client.PostAsJsonAsync(url, model)).Result;
                        if (postTask.IsSuccessStatusCode)
                        {
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                        else if (postTask.StatusCode == HttpStatusCode.BadRequest)
                        {
                            return Json(new { success = false , message = "400" }, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
                if (TempData.ContainsKey("Roles"))
                    ViewBag.Roles = TempData["Roles"];
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult EditPartial(string Id)
        {
            try
            {
                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    client.Timeout = TimeSpan.FromMinutes(60);
                    var url = _userSession.URL + "/api/BG/edit?Id=" + Id;
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<BusinessGroupDTO>(postTask.Content.ReadAsStringAsync().Result);
                        return PartialView(result);
                    }
                    return PartialView(new BusinessGroupDTO());
                }
            }
            catch
            {
                return PartialView(new EditModelDTO());
            }
        }
        [HttpPost]
        [ActionName("EditGroup")]
        public ActionResult Edit(BusinessGroupDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                        client.Timeout = TimeSpan.FromMinutes(60);
                        var url = _userSession.URL + "/api/BG/edit";
                        client.BaseAddress = new Uri(url);
                        var postTask = Task.Run(() => client.PostAsJsonAsync(url, model)).Result;
                        if (postTask.IsSuccessStatusCode)
                        {
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("DeleteGroup")]
        public ActionResult DeleteConfirmed(string Id, string GrpName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                        client.Timeout = TimeSpan.FromMinutes(60);
                        var url = _userSession.URL + "/api/BG/delete?Id=" + Id+"&GrpName="+GrpName;
                        client.BaseAddress = new Uri(url);
                        var postTask = Task.Run(() => client.GetAsync(url)).Result;
                        if (postTask.IsSuccessStatusCode)
                        {
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, statue = "-1" }, JsonRequestBehavior.AllowGet);
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
