using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using eInvoicing.API.Filters;
using eInvoicing.API.Models;
using ProductLicense;

namespace eInvoicing.API.Controllers
{
    [JwtAuthentication]
    public class DocumentSubmissionController : BaseController
    {
        private readonly IDocumentService _documentService;
        private readonly IErrorService _errorService;
        private readonly IAuthService _auth;
        private readonly IUserSession _userSession;
        public DocumentSubmissionController(IDocumentService documentService, 
            IErrorService errorService, IAuthService auth, IUserSession userSession)
        {
            _documentService = documentService;
            _errorService = errorService;
            _auth = auth;
            _userSession = userSession;
        }

        [LicenseAuthorization]
        [HttpPost, ActionName("_submit")]
        public IHttpActionResult SubmitDocument(SubmitDocumentRqDTO obj)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                _userSession.GetBusinessGroupId(this.GetBusinessGroupId());
                _errorService.GetTheConnectionString(this.OnActionExecuting());
                obj.documents.ForEach(i => i.documentTypeVersion = ConfigurationManager.AppSettings["TypeVersion"].ToLower() == "1.0" ? "1.0" : "0.9");
                var auth = _auth.token(_userSession.url, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = TimeSpan.FromMinutes(120);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = _userSession.submitServiceUrl + "api/InvoiceHasher/SubmitDocument";
                    client.BaseAddress = new Uri(url);
                    SubmitInput paramaters = new SubmitInput() { documents = obj.documents.ToList(), token = auth.access_token, pin = _userSession.pin, SRN = _userSession.SRN,
                        url = _userSession.submissionurl, docuemntTypeVersion = ConfigurationManager.AppSettings["TypeVersion"].ToLower()
                    };
                    var stringContent = new StringContent(JsonConvert.SerializeObject(paramaters), Encoding.UTF8, "application/json");
                    var postTask = client.PostAsync(url, stringContent);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<DocumentSubmissionDTO>(result.Content.ReadAsStringAsync().Result);
                        if (response.rejectedDocuments == null &&
                            response.acceptedDocuments == null && response.submissionId == null)
                        {
                            return Ok(new DocumentSubmissionDTO() { statusCode = response.statusCode });
                        }
                        else if (response.rejectedDocuments?.Count() == 0 && 
                            response.acceptedDocuments?.Count() == 0 && response.submissionId == null)
                        {
                            return Ok(new DocumentSubmissionDTO() { statusCode = "UnProcessableEntity" });
                        }
                        else
                        {
                            _errorService.InsertBulk(response.rejectedDocuments);
                            _documentService.UpdateDocuments(response, obj.submittedBy);
                            return Ok(response);
                        }
                    }
                    else
                    {
                        return Content(result.StatusCode, result.StatusCode.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [LicenseAuthorization]
        [HttpGet, ActionName("_autosubmit")]
        public IHttpActionResult AutoSubmitDocument(string submittedBy)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                _errorService.GetTheConnectionString(this.OnActionExecuting());
                _userSession.GetBusinessGroupId(this.GetBusinessGroupId());
                bool IsSignerError = false;
                DocumentSubmissionDTO Temp = new DocumentSubmissionDTO() { acceptedDocuments = new List<DocumentAcceptedDTO>(), rejectedDocuments = new List<DocumentRejectedDTO>() };
                var auth = _auth.token(_userSession.url, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                var _docs = _documentService.GetAllDocumentsToSubmit().ToList();
                _docs.ForEach(i => i.documentTypeVersion = ConfigurationManager.AppSettings["TypeVersion"].ToLower() == "1.0" ? "1.0" : "0.9");
                int totalPages = Convert.ToInt32(Math.Ceiling(_docs.Count() / 100.0));
                SubmitInput paramaters;
                for (int i = 0; i < totalPages; i++)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.Timeout = TimeSpan.FromMinutes(120);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var url = _userSession.submitServiceUrl + "api/InvoiceHasher/SubmitDocument";
                        client.BaseAddress = new Uri(url);
                        var _internalDocs = _documentService.GetAllDocumentsToSubmit();
                        if (_internalDocs.Count() < 100)
                        {
                            paramaters = new SubmitInput() {
                                documents = _internalDocs.ToList(),
                                token = auth.access_token,
                                pin = _userSession.pin,
                                SRN = _userSession.SRN,
                                url = _userSession.submissionurl,
                                docuemntTypeVersion = ConfigurationManager.AppSettings["TypeVersion"].ToLower()
                            };
                        }
                        else
                        {
                            paramaters = new SubmitInput() {
                                documents = _internalDocs.Take(100).ToList(),
                                token = auth.access_token,
                                pin = _userSession.pin,
                                SRN = _userSession.SRN,
                                url = _userSession.submissionurl,
                                docuemntTypeVersion = ConfigurationManager.AppSettings["TypeVersion"].ToLower()
                            };
                        }
                        var stringContent = new StringContent(JsonConvert.SerializeObject(paramaters), Encoding.UTF8, "application/json");
                        var postTask = client.PostAsync(url, stringContent);
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var response = JsonConvert.DeserializeObject<DocumentSubmissionDTO>(result.Content.ReadAsStringAsync().Result);
                            if (response != null)
                            {
                                //Temp.submissionId = response.submissionId;
                                if (response.acceptedDocuments != null)
                                    Temp.acceptedDocuments.AddRange(response.acceptedDocuments);
                                if (response?.rejectedDocuments != null)
                                {
                                    Temp.rejectedDocuments.AddRange(response.rejectedDocuments);
                                    _errorService.InsertBulk(response.rejectedDocuments);
                                }
                                if (Temp.rejectedDocuments?.Count() == 0 && Temp.acceptedDocuments?.Count() == 0 && response.statusCode?.ToLower() == "unprocessableentity")
                                    Temp.statusCode = "UnProcessableEntity";

                                if (response.rejectedDocuments == null && response.acceptedDocuments == null && response.submissionId == null)
                                {
                                    return Ok(new DocumentSubmissionDTO() { statusCode = response.statusCode });
                                }
                                _errorService.InsertBulk(response.rejectedDocuments);
                                _documentService.UpdateDocuments(response, submittedBy);
                            }
                        }
                        else
                        {
                            IsSignerError = true;
                        }
                        //return Ok(new DocumentSubmissionDTO() { statusCode = result.ReasonPhrase });
                    }
                }
                return Ok(!IsSignerError ? Temp : new DocumentSubmissionDTO() { statusCode = "-1"});
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
