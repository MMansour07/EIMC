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

namespace eInvoicing.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountVM model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                using (HttpClient client = new HttpClient())
                {
                    IUserSession _userSession = new UserSession();
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = TimeSpan.FromMinutes(60);
                    var url = _userSession.URL + "api/auth/login";
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.PostAsJsonAsync(url, model.LoginViewModel)).Result;
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
                            foreach (var item in response?.stringfiedRoles)
                            {
                                claims.Add(new Claim("Role", item));
                            }
                            foreach (var item in response?.stringfiedPermissions)
                            {
                                claims.Add(new Claim("Permission", item));
                            }
                            var identity = new ClaimsIdentity(claims, "ApplicationCookie");
                            Request.GetOwinContext().Authentication.SignIn(options, identity);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    else if (postTask.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ModelState.AddModelError("", "The license has been expired or doesn't belong to this client. Back to the product owner.");
                        return View(model);
                    }
                    ModelState.AddModelError("", "The user name or password you entered isn't correct. Try entering it again.");
                    return View(model);
                }
            }
            catch
            {
                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            return RedirectToAction("login", "account");
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