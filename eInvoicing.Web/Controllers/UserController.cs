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

    public class UserController : Controller
    {
        private readonly IUserSession _userSession;
        public UserController(IUserSession userSession)
        {
            _userSession = userSession;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                client.Timeout = TimeSpan.FromMinutes(60);
                var url = _userSession.URL + "/api/auth/getRoles";
                client.BaseAddress = new Uri(url);
                var postTask = Task.Run(() => client.GetAsync(url)).Result;
                if (postTask.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<List<DTO.RoleViewModel>>(postTask.Content.ReadAsStringAsync().Result);
                    TempData["Roles"] = ViewBag.Roles = result.Select(Role => new SelectListItem { Value = Role.Id, Text = Role.Name }).ToList();
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        [ActionName("GetUsers")]
        public ActionResult GetUsers()
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
                        var url = _userSession.URL + "/api/auth/getusers";
                        client.BaseAddress = new Uri(url);
                        var postTask = Task.Run(() => client.GetAsync(url)).Result;
                        if (postTask.IsSuccessStatusCode)
                        {
                            var users = JsonConvert.DeserializeObject<List<UserDTO>>(postTask.Content.ReadAsStringAsync().Result);
                            return Json(users, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateUser")]
        public ActionResult Create(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                        model.UserName = model.Email.Split('@')[0];
                        client.Timeout = TimeSpan.FromMinutes(60);
                        var url = _userSession.URL + "/api/auth/register";
                        client.BaseAddress = new Uri(url);
                        var postTask = Task.Run(() => client.PostAsJsonAsync(url, model)).Result;
                        if (postTask.IsSuccessStatusCode)
                        {
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
        [AllowAnonymous]
        [HttpGet]
        //[ActionName("EditUser")]
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
                    var url = _userSession.URL + "/api/auth/edit?Id=" + Id;
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<DTO.EditViewModel>(postTask.Content.ReadAsStringAsync().Result);
                        var rolelist = result.Roles.Select(Role => new SelectListItem { Value = Role.Id, Text = Role.Name, Selected = result.User.Roles.Contains(Role.Id) }).ToList();
                        ViewBag.Roles = rolelist;
                        return PartialView(result.User);
                    }
                    return PartialView(new EditModelDTO());
                }
            }
            catch
            {
                return PartialView(new EditModelDTO());
            }
        }
        [HttpPost]
        [ActionName("EditUser")]
        public ActionResult Edit(EditModelDTO model)
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
                        var url = _userSession.URL + "/api/auth/edit";
                        client.BaseAddress = new Uri(url);
                        var postTask = Task.Run(() => client.PutAsJsonAsync(url, model)).Result;
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

        [HttpPost, ActionName("DeleteUser")]
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
                        var url = _userSession.URL + "/api/auth/delete?Id=" + Id;
                        client.BaseAddress = new Uri(url);
                        var postTask = Task.Run(() => client.DeleteAsync(url)).Result;
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
