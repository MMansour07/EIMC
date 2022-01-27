using eInvoicing.API.Filters;
using eInvoicing.API.Models;
using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;

namespace eInvoicing.API.Controllers
{
    [JwtAuthentication]
    public class HangfireController : BaseController
    {
        private readonly IDocumentService _documentService;
        private readonly IErrorService _errorService;
        private readonly IAuthService _auth;
        private readonly IUserSession _userSession;
        public HangfireController(IDocumentService documentService, IAuthService auth, IUserSession userSession, IErrorService errorService)
        {
            _documentService = documentService;
            _auth = auth;
            _userSession = userSession;
            _errorService = errorService;
        }

        [HttpGet]
        [Route("api/hangfire/specifywhichactionschain")]
        public void SpecifyWhichActionsChain()
        {
            var simplePrinciple = (ClaimsPrincipal)HttpContext.Current.User;
            var identity = simplePrinciple?.Identity as ClaimsIdentity;
            string IsDBSync = ConfigurationManager.ConnectionStrings[identity?.FindFirst("IsDBSync")?.Value]?.ConnectionString?.ToLower();

            if (IsDBSync == "true")
                SyncDocumentsFromViewsToEIMC();
            else
                SubmitDocumentsPeriodically();
            //to be checked with tarek
        }
        private void SubmitDocumentsPeriodically()
        {
            try
            {
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
                        client.Timeout = TimeSpan.FromMinutes(60);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var url = _userSession.submitServiceUrl + "api/InvoiceHasher/SubmitDocument";
                        client.BaseAddress = new Uri(url);
                        var _internalDocs = _documentService.GetAllDocumentsToSubmit();
                        if (_internalDocs.Count() < 100)
                        {
                            paramaters = new SubmitInput() { documents = _internalDocs.ToList(), token = auth.access_token, url = _userSession.submissionurl };
                        }
                        else
                        {
                            paramaters = new SubmitInput() { documents = _internalDocs.Take(100).ToList(), token = auth.access_token, url = _userSession.submissionurl };
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
                                if (response.acceptedDocuments != null)
                                    Temp.acceptedDocuments.AddRange(response.acceptedDocuments);
                                if (response?.rejectedDocuments != null)
                                {
                                    Temp.rejectedDocuments.AddRange(response.rejectedDocuments);
                                    _errorService.InsertBulk(response.rejectedDocuments);
                                }
                                _documentService.UpdateDocuments(response, "Auto");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SyncDocumentsFromViewsToEIMC()
        {
            string commandText = "EXEC [dbo].[SP_SyncDataFromViewsToTBLs]";
            RunCommandAsynchronously(commandText, GetConnectionString());
        }
        private string GetConnectionString()
        {
            // To avoid storing the connection string in your code,
            // you can retrieve it from a configuration file.

            // If you have not included "Asynchronous Processing=true" in the
            // connection string, the command is not able
            // to execute asynchronously.
            return ConfigurationManager.ConnectionStrings["eInvoicing_CS"].ToString();
        }
        private void RunCommandAsynchronously(string commandText, string connectionString)
        {
            // Given command text and connection string, asynchronously execute
            // the specified command against the connection. For this example,
            // the code displays an indicator as it is working, verifying the
            // asynchronous behavior.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    //int count = 0;
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    IAsyncResult result = command.BeginExecuteNonQuery();
                    if (result.IsCompleted)
                    {
                        Debug.WriteLine("Command complete. Affected {0} rows.", command.EndExecuteNonQuery(result));
                        SubmitDocumentsPeriodically();
                    }
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("Error ({0}): {1}", ex.Number, ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Debug.WriteLine("Error: {0}", ex.Message);
                }
                catch (Exception ex)
                {
                    // You might want to pass these errors
                    // back out to the caller.
                    Debug.WriteLine("Error: {0}", ex.Message);
                }
            }
        }

    }
}