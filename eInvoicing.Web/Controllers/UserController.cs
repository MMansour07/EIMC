using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
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
        [HttpGet]
        public ActionResult Index()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                var url = _userSession.URL + "/api/auth/getRoles";
                client.BaseAddress = new Uri(url);
                var postTask = Task.Run(() => client.GetAsync(url)).Result;
                if (postTask.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<List<DTO.RoleViewModel>>(postTask.Content.ReadAsStringAsync().Result);
                    TempData["Roles"] = ViewBag.Roles = result.Select(Role => new SelectListItem { Value = Role.Id, Text = Role.Name }).ToList();
                    var _url = _userSession.URL + "/api/BG/GetGroups";
                    using (HttpClient _client = new HttpClient())
                    {
                        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                        _client.BaseAddress = new Uri(_url);
                        var _postTask = Task.Run(() => _client.GetAsync(_url)).Result;
                        if (_postTask.IsSuccessStatusCode)
                        {
                            var _result = JsonConvert.DeserializeObject<List<BusinessGroupDTO>>(_postTask.Content.ReadAsStringAsync().Result);
                            TempData["Groups"] = ViewBag.Groups = _result.Select(Group => new SelectListItem { Value = Group.Id, Text = Group.GroupName }).ToList();
                            return View();
                        }
                    }
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
                    var BusinessGroupId = ClaimsPrincipal.Current.FindFirst("BusinessGroupId").Value;
                    var LoggedinUserName = ClaimsPrincipal.Current.Identity.Name;
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                        client.Timeout = TimeSpan.FromMinutes(60);
                        var url = _userSession.URL + "/api/auth/getusers?BusinessGroupId="+ BusinessGroupId+ "&LoggedinUserName=" + LoggedinUserName;
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
                    var LoggedinUserName = ClaimsPrincipal.Current.Identity.Name;
                    using (HttpClient client = new HttpClient())
                    {
                        if (LoggedinUserName.ToLower() != "superadmin")
                        {
                            model.BusinessGroupId = ClaimsPrincipal.Current.FindFirst("BusinessGroupId").Value;
                        }
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
                        else if (postTask.StatusCode == HttpStatusCode.BadRequest)
                        {
                            return Json(new { success = false , message = "400" }, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
                if (TempData.ContainsKey("Roles"))
                    ViewBag.Roles = TempData["Roles"];

                if (TempData.ContainsKey("Groups"))
                    ViewBag.Roles = TempData["Groups"];


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
                    var url = _userSession.URL + "/api/auth/edit?Id=" + Id;
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<DTO.EditViewModel>(postTask.Content.ReadAsStringAsync().Result);
                        var rolelist = result.Roles.Select(Role => new SelectListItem { Value = Role.Id, Text = Role.Name, Selected = result.User.Roles.Contains(Role.Id) }).ToList();
                        ViewBag.Roles = rolelist;
                        var _url = _userSession.URL + "/api/BG/GetGroups";
                        using (HttpClient _client = new HttpClient())
                        {
                            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                            _client.BaseAddress = new Uri(_url);
                            var _postTask = Task.Run(() => _client.GetAsync(_url)).Result;
                            if (_postTask.IsSuccessStatusCode)
                            {
                                var _result = JsonConvert.DeserializeObject<List<BusinessGroupDTO>>(_postTask.Content.ReadAsStringAsync().Result);
                                ViewBag.Groups = _result.Select(Group => new SelectListItem { Value = Group.Id, Text = Group.GroupName, Selected = _result.Select(x=> x.Id).Contains(result.User.BusinessGroupId)}).ToList();
                            }
                        }
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
        [ActionName("DeleteUser")]
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
        [HttpGet]
        [ActionName("myprofile")]
        public ActionResult MyProfile()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    client.Timeout = TimeSpan.FromMinutes(60);
                    var url = _userSession.URL + "/api/auth/edit?Id=" + ClaimsPrincipal.Current.Identity.GetUserId();
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<DTO.EditViewModel>(postTask.Content.ReadAsStringAsync().Result);
                        return View(result.User);
                    }
                    return View(new DTO.EditViewModel());
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //return View(new EditModelDTO());
            }
        }
    }
}
