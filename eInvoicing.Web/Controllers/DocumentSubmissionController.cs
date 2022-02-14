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
                        if (response != null)
                        {
                            if (!string.IsNullOrEmpty(response.statusCode))
                            {
                                if (response.statusCode?.ToLower() == "unprocessableentity")
                                {
                                    return Json(new { message = "Unprocessable Entity, You must wait for 10 mins to resend this document.", status = "5", data = response }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { message = response.statusCode, status = "2", data = response }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json(new { message = postTask.StatusCode.ToString(), status = "1", data = response }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                            return Json(new { message = postTask.StatusCode.ToString(), status = "3", data = response }, JsonRequestBehavior.AllowGet);
                    }
                    else if (postTask.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return Json(new { message = postTask.ReasonPhrase, status = "401", data = new DocumentSubmissionDTO() }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { message = postTask.StatusCode.ToString(), status = "2", data = new DocumentSubmissionDTO() }, JsonRequestBehavior.AllowGet);
                    // 1 --> Sucess
                    // 2 --> Integration error ----> ETA APIs
                    // 3 --> Integration Error ----> Internal APIs
                    // 4 --> Inner Exception
                    // 5 --> Unprocessable Entity
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
                        if (response.statusCode != "-1")
                        {
                            if (response.acceptedDocuments?.Count == 0 && response.rejectedDocuments?.Count == 0 && response.statusCode?.ToLower() == "unprocessableentity")
                            {
                                return Json(new { message = "Unprocessable Entity, You must wait for 10 mins to resend these documents.", status = "5", data = response }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.statusCode))
                                {
                                    return Json(new { message = postTask.StatusCode, status = "1", data = response }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { message = response.statusCode, status = "5", data = response }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        else
                            return Json(new { message = response.statusCode.ToString(), status = "2", data = response }, JsonRequestBehavior.AllowGet);
                    }
                    else if (postTask.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return Json(new { message = postTask.ReasonPhrase, status = "401", data = new DocumentSubmissionDTO() }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { message = postTask.StatusCode.ToString(), status = "3", data = new DocumentSubmissionDTO() }, JsonRequestBehavior.AllowGet);
                    // 1 --> Sucess
                    // 2 --> Integration error ----> ETA APIs
                    // 3 --> Integration Error ----> Internal APIs
                    // 4 --> Inner Exception
                    // 5 --> Unprocessable Entity
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.ToString(), status = "4", data = new DocumentSubmissionDTO() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
