using eInvoicing.DTO;
using eInvoicing.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eInvoicing.Web.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IUserSession _userSession;
        public RoleController(IUserSession userSession)
        {
            _userSession = userSession;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    client.Timeout = TimeSpan.FromMinutes(60);
                    var url = _userSession.URL + "api/auth/getPerimssions";
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<PrivilegeViewModel>(postTask.Content.ReadAsStringAsync().Result);
                        //TempData["Privileges"] = ViewBag.Privileges = result.Privileges.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Controller }).ToList();
                        TempData["Permissions"] = ViewBag.Permissions = result.Permissions.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Id }).ToList();
                        return View();
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        [HttpGet]
        [ActionName("GetRoles")]
        public ActionResult GetRoles()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    client.Timeout = TimeSpan.FromMinutes(60);
                    var url = _userSession.URL + "api/auth/getRoles";
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        if (postTask.IsSuccessStatusCode)
                        {
                            var roles = JsonConvert.DeserializeObject<List<DTO.RoleViewModel>>(postTask.Content.ReadAsStringAsync().Result);
                            return Json(roles, JsonRequestBehavior.AllowGet);
                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        //[ActionName("CreateRole")]
        public ActionResult Create(string message = "")
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                client.Timeout = TimeSpan.FromMinutes(60);
                var url = _userSession.URL + "api/auth/getPrivileges";
                client.BaseAddress = new Uri(url);
                var postTask = Task.Run(() => client.GetAsync(url)).Result;
                if (postTask.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<PrivilegeViewModel>(postTask.Content.ReadAsStringAsync().Result);
                    ViewBag.Privileges = result.Privileges.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Controller }).ToList();
                    ViewBag.Permissions = result.Permissions.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Action }).ToList();
                    return View();
                }
            }
            return View();
        }
        
        [HttpPost]
        [ActionName("CreateRole")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.RoleViewModel model)
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
                        var url = _userSession.URL + "api/auth/createRole";
                        client.BaseAddress = new Uri(url);
                        var postTask = Task.Run(() => client.PostAsJsonAsync(url, model)).Result;
                        if (postTask.IsSuccessStatusCode)
                        {
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                if (TempData.ContainsKey("Privileges"))
                    ViewBag.Privileges = TempData["Privileges"];
                if (TempData.ContainsKey("Permissions"))
                    ViewBag.Permissions = TempData["Permissions"];
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult editpartial(string Id)
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
                    var url = _userSession.URL + "api/auth/editRole?Id=" + Id;
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<EditRoleModel>(postTask.Content.ReadAsStringAsync().Result);
                        //ViewBag.Privileges = result.Privileges.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Controller, Selected = result.Role.Privileges.Contains(x.Id.ToString()) }).ToList();
                        ViewBag.Permissions = result.Permissions.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Id, Selected = result.Role.Permissions.Contains(x.Id.ToString()) }).ToList();
                        return PartialView(result.Role);
                    }
                    return PartialView(new RoleDTO());
                }
            }
            catch
            {
                return PartialView(new RoleDTO());
            }
        }
        [HttpPost]
        [ActionName("EditRole")]
        public ActionResult Edit(RoleDTO model)
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
                        var url = _userSession.URL + "api/auth/editRole";
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
        [ActionName("DeleteRole")]
        public ActionResult DeleteConfirmed(string Id)
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
                        var url = _userSession.URL + "api/auth/deleteRole?Id=" + Id;
                        client.BaseAddress = new Uri(url);
                        var postTask = Task.Run(() => client.GetAsync(url)).Result;
                        if (postTask.IsSuccessStatusCode)
                        {
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}