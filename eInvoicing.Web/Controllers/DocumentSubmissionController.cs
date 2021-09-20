using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation;
using eInvoicing.Service.Helper;
using eInvoicing.Web.Helper;
using eInvoicing.Web.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace eInvoicing.Web.Controllers
{
    // to avoid sending token client side 
    // Seeking for optimal solution
    [Authorize]
    public class DocumentSubmissionController : Controller
    {
        private readonly IUserSession _userSession;
        public DocumentSubmissionController(IUserSession userSession)
        {
            _userSession = userSession;
        }
        [HttpPost]
        [ActionName("submit")]
        public ActionResult SubmitDocuments(List<DocumentVM> obj)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    client.Timeout = TimeSpan.FromMinutes(60);
                    var url = _userSession.URL + "api/documentsubmission/_submit";
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.PostAsJsonAsync(url, new SubmitDocumentRqDTO(){ submittedBy = User.Identity.Name, documents = obj})).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<DocumentSubmissionDTO>(postTask.Content.ReadAsStringAsync().Result);
                        if(string.IsNullOrEmpty(response.statusCode))
                        return Json(new {message = postTask.StatusCode.ToString(), status = "1", data = response}, JsonRequestBehavior.AllowGet);
                        else
                        return Json(new {message = postTask.StatusCode.ToString(), status = "2", data = response}, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { message = postTask.StatusCode.ToString(), status = "3", data = new DocumentSubmissionDTO() }, JsonRequestBehavior.AllowGet);
                    // 1 --> Sucess
                    // 2 --> External integration error
                    // 3 --> Internal Integration Error
                    // 4 --> Internal server error
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.ToString(), status = "4", data = new DocumentSubmissionDTO() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("_autosubmit")]
        public ActionResult AutoSubmitDocuments()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    client.Timeout = TimeSpan.FromMinutes(60);
                    client.DefaultRequestHeaders.Clear();
                    var url = _userSession.URL + "api/documentsubmission/_autosubmit?SubmittedBy="+ User.Identity.Name;
                    client.BaseAddress = new Uri(url);
                    var postTask = Task.Run(() => client.GetAsync(url)).Result;
                    if (postTask.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<DocumentSubmissionDTO>(postTask.Content.ReadAsStringAsync().Result);
                        if (string.IsNullOrEmpty(response.statusCode))
                            return Json(new { message = postTask.StatusCode.ToString(), status = "1", data = response }, JsonRequestBehavior.AllowGet);
                        else
                            return Json(new { message = response.statusCode.ToString(), status = "2", data = response }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { message = postTask.StatusCode.ToString(), status = "3", data = new DocumentSubmissionDTO() }, JsonRequestBehavior.AllowGet);
                    // 1 --> Sucess
                    // 2 --> External integration error
                    // 3 --> Internal Integration Error
                    // 4 --> Internal server error
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.ToString(), status = "4", data = new DocumentSubmissionDTO() }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
