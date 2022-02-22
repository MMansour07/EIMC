using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using eInvoicing.Web.Models;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using eInvoicing.DTO;
using System.Security.Claims;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using NLog;

namespace eInvoicing.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserSession _userSession;
        //private readonly ILogger<AccountController> _logger;

        public AccountController(IUserSession userSession)
        {
            //_logger = logger;
            _userSession = userSession;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("login")]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    IUserSession _userSession = new UserSession();
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = TimeSpan.FromHours(2);
                    var url = _userSession.URL + "api/auth/login";
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.PostAsJsonAsync(url, model)).Result;
                    logger.Info(postTask.IsSuccessStatusCode);
                    if (postTask.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<UserDTO>(postTask.Content.ReadAsStringAsync().Result);
                        AuthenticationProperties options = new AuthenticationProperties();
                        options.AllowRefresh = true;
                        options.IsPersistent = true;
                        options.ExpiresUtc = DateTime.UtcNow.AddDays(1);
                        List<Claim> claims = new List<Claim>();
                        if (response != null)
                        {
                            claims.Add(new Claim(ClaimTypes.Name, response?.UserName));
                            claims.Add(new Claim(ClaimTypes.NameIdentifier, response?.Id));
                            claims.Add(new Claim("Title", response?.Title));
                            claims.Add(new Claim("AcessToken", string.Format(response?.Token)));
                            claims.Add(new Claim("FullName", response?.FullName));
                            claims.Add(new Claim("BusinessGroupId", response?.BusinessGroupId));
                            claims.Add(new Claim("Token", response?.Token));
                            claims.Add(new Claim("SRN", response?.SRN));
                            claims.Add(new Claim("IsDBSync", response?.IsDBSync.ToString()));
                            foreach (var item in response?.stringfiedRoles)
                            {
                                claims.Add(new Claim("Role", item));
                            }
                            var identity = new ClaimsIdentity(claims, "ApplicationCookie");
                            Request.GetOwinContext().Authentication.SignIn(options, identity);
                            return Json(new { success = true, returnUrl = model.ReturnUrl }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { success = false, status = "0" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception --> " + ex.Message.ToString() + "[Date Time] = " + DateTime.Now.ToString() + "[Date Time (UTC)] = " + DateTime.UtcNow.ToString());
                return Json(new { success = false, status = "-1" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Request.GetOwinContext()
       .Authentication
       .SignOut(HttpContext.GetOwinContext()
                           .Authentication.GetAuthenticationTypes()
                           .Select(o => o.AuthenticationType).ToArray());
            //Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            return RedirectToAction("login", "account");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        model.Username = ClaimsPrincipal.Current.Identity.GetUserName();
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                        client.Timeout = TimeSpan.FromMinutes(60);
                        var url = _userSession.URL + "/api/auth/changepassword";
                        client.BaseAddress = new Uri(url);
                        var postTask = Task.Run(() => client.PostAsJsonAsync(url, model)).Result;
                        if (postTask.IsSuccessStatusCode)
                        {
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                        else if (postTask.StatusCode == HttpStatusCode.BadRequest)
                        {
                            return Json(new { success = false, message = "400" }, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Master");
        }
    }
}