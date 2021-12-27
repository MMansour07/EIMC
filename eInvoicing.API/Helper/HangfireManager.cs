using eInvoicing.API.Models;
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
        public static void SyncDocumentsFromViewsToEIMC()
        {
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["eInvoicing_CS"].ToString()))
                {
                    myConnection.Open();
                    SqlCommand comm = myConnection.CreateCommand();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandTimeout = 600;
                    comm.CommandText = "SP_SyncDataFromViewsToTBLs";
                    var _rowsaffected = comm.ExecuteNonQuery();
                }
            }

            catch
            {
                // to do --> logs into file
            }
        }
        public static void UpdateDocumentsStatusFromETAToEIMC()
        {
            try
            {
                IdentityContext identitycon = new IdentityContext();
                ITaxpayerRepository _taxpayerrepo = new TaxpayerRepository(identitycon);
                ITaxpayerService taxpayerService = new TaxpayerService(_taxpayerrepo);
                IUserSession _userSession = new UserSession(taxpayerService);
                ApplicationContext con = new ApplicationContext();
                IDocumentRepository _repo = new DocumentRepository(con);
                IDocumentService obj = new DocumentService(_repo);
                IAuthService _auth = new AuthService();
                var auth =  _auth.token(_userSession.url, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                obj.GetRecentDocuments_ETA(_userSession.submissionurl, auth.access_token, 2000);
            }

            catch
            {
                // to do --> logs into file
            }
        }
        public static void SubmitDocumentsPeriodically()
        {
            try
            {
                ApplicationContext con = new ApplicationContext();
                IdentityContext identitycon = new IdentityContext();
                IDocumentRepository _repo = new DocumentRepository(con);
                IErrorReposistory _errorrepo = new ErrorRepository(con);
                ITaxpayerRepository _taxpayerrepo = new TaxpayerRepository(identitycon);
                ITaxpayerService taxpayerService = new TaxpayerService(_taxpayerrepo);
                IDocumentService _documentService = new DocumentService(_repo);
                IErrorService _errorService = new ErrorService(_errorrepo);
                IAuthService _auth = new AuthService();
                IUserSession _userSession = new UserSession(taxpayerService);
                DocumentSubmissionDTO Temp = new DocumentSubmissionDTO() { acceptedDocuments = new List<DocumentAcceptedDTO>(), rejectedDocuments = new List<DocumentRejectedDTO>() };
                var auth = _auth.token(_userSession.url, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                var _docs = _documentService.GetAllDocumentsToSubmit().ToList();
                _docs.ForEach(i => i.documentTypeVersion = ConfigurationManager.AppSettings["TypeVersion"].ToLower() == "1.0" ? "1.0" : "0.9");
                int totalPages = Convert.ToInt32(Math.Ceiling(_docs.Count() / 30.0));
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
                        if (_internalDocs.Count() < 30)
                        {
                            paramaters = new SubmitInput() { documents = _internalDocs.ToList(), token = auth.access_token, url = _userSession.submissionurl };
                        }
                        else
                        {
                            paramaters = new SubmitInput() { documents = _internalDocs.Take(30).ToList(), token = auth.access_token, url = _userSession.submissionurl };
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
        public static void EIMCBackupPeriodically()
        {
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["eInvoicing_CS"].ToString()))
                {
                    myConnection.Open();
                    string SQLQuery = @"BACKUP DATABASE EIMC_Preprod TO DISK = '"+ ConfigurationManager.AppSettings["Backup_Path"] + DateTime.Now.ToString("yyyyMMdd") + ".bak'";
                    SqlCommand command = new SqlCommand(SQLQuery, myConnection);
                    command.CommandTimeout = 600;
                    var output = command.ExecuteReader();
                    output.Close();
                    myConnection.Close();
                }
            }
            catch
            {
                // to do --> logs into file
            }
        }
    }
}