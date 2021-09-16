using eInvoicing.DTO;
using eInvoicing.Persistence;
using eInvoicing.Repository.Contract;
using eInvoicing.Repository.Implementation;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation;
using Hangfire;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace eInvoicing.API.Helper
{
    public class HangfireManager
    {
        private static string loginUrl { get; set; }
        private static string APIUrl { get; set; }
        private static string client_id { get; set; }
        private static string client_secret { get; set; }
        private static string submitServiceUrl { get; set; }
        private static string submissionurl { get; set; }
        public static void Sync()
        {
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["eInvoicing_CS"].ToString()))
                {
                    myConnection.Open();
                    SqlCommand comm = myConnection.CreateCommand();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandTimeout = 300;
                    comm.CommandText = "SP_SyncDataFromViewsToTBLs";
                    var _rowsaffected = comm.ExecuteNonQuery();
                }
            }

            catch
            {
                // to do --> logs into file
            }
        }
        public static void SyncFromETAToDB()
        {
            try
            {
                if (ConfigurationManager.AppSettings["Environment"].ToLower() == "preprod")
                {
                    loginUrl = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                    APIUrl = ConfigurationManager.AppSettings["apiBaseUrl"];
                    submitServiceUrl = ConfigurationManager.AppSettings["submitSrvBaseUrl"];
                    client_id = ConfigurationManager.AppSettings["client_id"];
                    client_secret = ConfigurationManager.AppSettings["client_secret"];
                    submissionurl = ConfigurationManager.AppSettings["apiBaseUrl"];
                }
                else
                {
                    loginUrl = ConfigurationManager.AppSettings["ProdidSrvBaseUrl"];
                    APIUrl = ConfigurationManager.AppSettings["ProdapiBaseUrl"];
                    client_id = ConfigurationManager.AppSettings["Prod_client_id"];
                    submitServiceUrl = ConfigurationManager.AppSettings["ProdsubmitSrvBaseUrl"];
                    client_secret = ConfigurationManager.AppSettings["Prod_client_secret"];
                    submissionurl = ConfigurationManager.AppSettings["ProdapiBaseUrl"];
                }
                ApplicationContext con = new ApplicationContext();
                IDocumentRepository _repo = new DocumentRepository(con);
                IDocumentService obj = new DocumentService(_repo);
                IAuthService _auth = new AuthService();
                var auth =  _auth.token(loginUrl, "client_credentials", client_id, client_secret, "InvoicingAPI");
                obj.GetRecentDocuments_ETA(APIUrl, auth.access_token, 2000);
            }

            catch
            {
                // to do --> logs into file
            }
        }
        public static void AutoSubmission()
        {
            try
            {
                if (ConfigurationManager.AppSettings["Environment"].ToLower() == "preprod")
                {
                    loginUrl = ConfigurationManager.AppSettings["idSrvBaseUrl"];
                    APIUrl = ConfigurationManager.AppSettings["apiBaseUrl"];
                    submitServiceUrl = ConfigurationManager.AppSettings["submitSrvBaseUrl"];
                    client_id = ConfigurationManager.AppSettings["client_id"];
                    client_secret = ConfigurationManager.AppSettings["client_secret"];
                    submissionurl = ConfigurationManager.AppSettings["apiBaseUrl"];
                }
                else
                {
                    loginUrl = ConfigurationManager.AppSettings["ProdidSrvBaseUrl"];
                    APIUrl = ConfigurationManager.AppSettings["ProdapiBaseUrl"];
                    client_id = ConfigurationManager.AppSettings["Prod_client_id"];
                    submitServiceUrl = ConfigurationManager.AppSettings["ProdsubmitSrvBaseUrl"];
                    client_secret = ConfigurationManager.AppSettings["Prod_client_secret"];
                    submissionurl = ConfigurationManager.AppSettings["ProdapiBaseUrl"];
                }
                ApplicationContext con = new ApplicationContext();
                IDocumentRepository _repo = new DocumentRepository(con);
                IErrorReposistory _errorrepo = new ErrorRepository(con);
                IDocumentService _documentService = new DocumentService(_repo);
                IErrorService _errorService = new ErrorService(_errorrepo);
                IAuthService _auth = new AuthService();
                var auth = _auth.token(loginUrl, "client_credentials", client_id, client_secret, "InvoicingAPI");
                CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = submitServiceUrl + "api/InvoiceHasher/SubmitDocument";
                    client.BaseAddress = new Uri(url);
                    var _docs = _documentService.GetAllDocumentsToSubmit().Take(100).ToList();
                    _docs.ForEach(i => i.documentTypeVersion = ConfigurationManager.AppSettings["TypeVersion"].ToLower() == "1.0" ? "1.0" : "0.9");
                    SubmitInput paramaters = new SubmitInput() { documents = _docs, token = auth.access_token, url = submissionurl };
                    var stringContent = new StringContent(JsonConvert.SerializeObject(paramaters), Encoding.UTF8, "application/json");
                    var postTask = client.PostAsync(url, stringContent);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<DocumentSubmissionDTO>(result.Content.ReadAsStringAsync().Result);
                        if (response != null)
                        {
                            _errorService.InsertBulk(response.rejectedDocuments);
                            _documentService.UpdateDocuments(response, null);
                        }
                    }
                }
            }

            catch
            {
                // to do --> logs into file
            }
        }
    }
}