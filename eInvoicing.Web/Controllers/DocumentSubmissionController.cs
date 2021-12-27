using eInvoicing.DTO;
using eInvoicing.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;


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
                        if (string.IsNullOrEmpty(response.statusCode))
                        {
                            if (response != null)
                            {
                                return Json(new { message = postTask.StatusCode.ToString(), status = "1", data = response }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { message = "Unprocessable Entity, You must wait for 10 mins to resend this document to ensure the modification.", status = "5", data = response }, JsonRequestBehavior.AllowGet);
                            }
                        }
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
        [ActionName("auto_submit")]
        public ActionResult AutoSubmitDocuments()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    client.Timeout = TimeSpan.FromMinutes(60);
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
