using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.Repository.Contract;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using eInvoicing.Service.Helper.Extension;
using System.Linq.Dynamic;
using System.Data.Entity;
using eInvoicing.Service.Helper;
using System.Diagnostics;
using System.Text;

namespace eInvoicing.Service.AppService.Implementation
{
    public class DocumentService :  IDocumentService
    {
        private readonly IDocumentRepository repository;
        private readonly IValidationStepRepository _validationStepRepository;
        private readonly IStepErrorRepository _stepErrorRepository;
        private List<string> IdsStack;
        private List<string> IdsStack2;

        public DocumentService(IDocumentRepository _repository, IValidationStepRepository validationStepRepository,
            IStepErrorRepository stepErrorRepository)
        {
            this.repository = _repository;
            this._validationStepRepository = validationStepRepository;
            this._stepErrorRepository = stepErrorRepository;
        }
        public void GetTheConnectionString(string ConnectionString)
        {
            this.repository.GetTheConnectionString(ConnectionString);
            this._validationStepRepository.GetTheConnectionString(ConnectionString);
            this._stepErrorRepository.GetTheConnectionString(ConnectionString);
        }
        public HttpResponseMessage GetDocumentPrintOut(string URL, string Key, string uuid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
                URL += "documents/" + uuid + "/pdf";
                client.BaseAddress = new Uri(URL);
                var postTask = client.GetAsync(URL);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return result;
                }
            }
                    
            return null;
        }
        public void GetRecentDocuments_ETA(string URL, string Key, int DailyInvoicesAverage)
        {
            try
            {
                IdsStack = new List<string>();
                for (int i = 0; i < DailyInvoicesAverage/100; i++)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.Timeout = TimeSpan.FromMinutes(60);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.BaseAddress = new Uri(URL + "documents/recent?pageNo=" + (i + 1) + "&pageSize=100");
                        var postTask = client.GetAsync(URL + "documents/recent?pageNo=" + (i + 1) + "&pageSize=100");
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var res = JsonConvert.DeserializeObject<GetRecentDocumentsResponse>(result.Content.ReadAsStringAsync().Result);
                            UpdateDocumentsStatus(res);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: {0}", ex.Message);
            }
        }

        public void GetReceivedDocuments(string URL, string Key, int DailyInvoicesAverage)
        {
            try
            {
                IdsStack2 = new List<string>();
                for (int i = 0; i < DailyInvoicesAverage / 100; i++)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.Timeout = TimeSpan.FromMinutes(60);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.BaseAddress = new Uri(URL + "documents/recent?pageNo=" + (i + 1) + "&pageSize=100");
                        var postTask = client.GetAsync(URL + "documents/recent?pageNo=" + (i + 1) + "&pageSize=100");
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var res = JsonConvert.DeserializeObject<GetRecentDocumentsResponse>(result.Content.ReadAsStringAsync().Result);
                            InsertReceivedDocuments(res, Key, URL);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: {0}", ex.Message);
            }
        }
        public GetRecentDocumentsResponse GetRecentDocuments_ETA2(string URL, string Key, int pageNo, int pageSize)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("PageSize", pageSize);
                    //client.DefaultRequestHeaders.Add("PageNo", pageNo);
                    URL += "documents/recent?pageNo=" + pageNo + "&pageSize=" + pageSize + "";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var res = JsonConvert.DeserializeObject<GetRecentDocumentsResponse>(result.Content.ReadAsStringAsync().Result);
                        return res;
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public GetDocumentResponse GetDocument_ETA(string URL, string Key, string uuid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = TimeSpan.FromMinutes(60);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "documents/"+uuid+ "/details";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var res = JsonConvert.DeserializeObject<GetDocumentResponse>(result.Content.ReadAsStringAsync().Result);
                        res.StatusCode = System.Net.HttpStatusCode.OK;
                        return res;

                        //var GetDocumentResponseJSON = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                        //var document = GetDocumentResponseJSON.SelectToken("document");
                        //var documentProps = JObject.Parse(document.ToString()).Properties();
                        //documentProps.Where(attr => attr.Name.Equals("taxAuthorityDocument")).ToList().ForEach(attr => attr.Remove());
                        //GetDocumentResponseJSON.Properties().Where(attr => attr.Name.Equals("document")).ToList().ForEach(attr => attr.Remove());
                        //var documentJsonStr = JsonConvert.SerializeObject(new JObject(documentProps));
                        //GetDocumentResponseJSON.Add("document", JObject.Parse(documentJsonStr));
                        //var response = JsonConvert.DeserializeObject<GetDocumentResponse>(GetDocumentResponseJSON.ToString());
                        //response.StatusCode = System.Net.HttpStatusCode.OK;
                        //return response;
                    }
                    else 
                    {
                        return new GetDocumentResponse() { StatusCode = result.StatusCode};
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CancelDocument(string URL, string Key, string uuid, string reason)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = TimeSpan.FromMinutes(60);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = URL + "documents/state/" + uuid + "/state";
                    client.BaseAddress = new Uri(URL + "documents/state/" + uuid + "/state");
                    CancelDocumentRq cancelDocumentRq = new CancelDocumentRq() {status = "cancelled", reason = reason};
                    var stringContent = new StringContent(JsonConvert.SerializeObject(cancelDocumentRq), Encoding.UTF8, "application/json");
                    var postTask = client.PutAsync(url, stringContent);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        if (RequestDocumentCancellation(uuid, reason))
                            return true;
                        else
                            return false;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RejectDocument(string URL, string Key, string uuid, string reason)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    URL += "documents/" + uuid + "/state";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PutAsJsonAsync(URL, new
                    {
                        status = "rejected",
                        reason = reason
                    });
                    postTask.Wait();
                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return 1;
                    }
                    return 0;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public int DeclineDocumentCancellation(string URL, string Key, string uuid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    URL += "documents/state/"+uuid+"/decline/cancelation";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PutAsync(URL,null);
                    postTask.Wait();
                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return 1;
                    }
                    return 0;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public int DeclineDocumentRejection(string URL, string Key, string uuid)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    URL += "documents/state/" + uuid + "/decline/rejection";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PutAsync(URL, null);
                    postTask.Wait();
                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return 1;
                    }
                    return 0;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public IEnumerable<DocumentVM> GetAllDocumentsToSubmit()
        {
            return repository.Get(i => i.Status.ToLower() == "new" || i.Status.ToLower() == "failed" ||
            i.Status.ToLower() == "updated", m => m.OrderBy(x => x.DateTimeIssued), "InvoiceLines").ToList().Select(x => x.ToDocumentVM());
        }

        public PagedList<DocumentVM> GetPendingDocuments(int pageNumber, int pageSize, DateTime fromDate, 
            DateTime toDate, string searchValue, string sortColumnName, string sortDirection, string status)
        {
            toDate = toDate.AddDays(1);
            IQueryable<Document> docs; 
            if (string.IsNullOrEmpty(status) || status.ToLower() == "all")
            {
                 docs = repository.Get(i => (i.Status.ToLower() == "new" || i.Status.ToLower() == "failed" 
                 || i.Status.ToLower() == "updated") && (i.DateTimeIssued >= fromDate.Date && i.DateTimeIssued <= toDate.Date), m => m.OrderByDescending(x => x.DateTimeIssued), "InvoiceLines");
            }
            else
            {
                 docs = repository.Get(i => i.Status.ToLower() == status.ToLower() && (i.DateTimeIssued >= fromDate.Date && i.DateTimeIssued <= toDate.Date), m => m.OrderByDescending(x => x.DateTimeIssued), "InvoiceLines");
            }
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                searchValue = searchValue.ToLower().Replace("/", "");
                docs = docs.Where(x => x.Id.ToString().Contains(searchValue) || x.Status.ToString().ToLower().Contains(searchValue) || searchValue.Contains(x.DocumentType.ToLower()) || 
                x.DocumentTypeVersion.ToLower().Contains(searchValue) || x.TotalSalesAmount.ToString().ToLower().Contains(searchValue) ||
                x.TotalItemsDiscountAmount.ToString().ToLower().Contains(searchValue) || x.TotalAmount.ToString().ToLower().Contains(searchValue) ||
                x.ReceiverName.ToLower().Contains(searchValue) || x.ReceiverType.ToLower().Contains(searchValue));
            }
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                docs = docs.OrderBy(sortColumnName + " " + sortDirection);
            }
            var temp = PagedList<Document>.Create(docs, pageNumber, pageSize, docs.Count());
            return new PagedList<DocumentVM>(temp.Select(x => x.ToDocumentVM()).ToList(), docs.Count(), pageNumber, pageSize, docs.Count());
        }
        public int GetPendingCount()
        {
            return repository.Get(i => i.Status.ToLower() == "new" || i.Status.ToLower() == "failed" 
            || i.Status.ToLower() == "updated", null, "").Count();
        }
        public int GetSubmittedCount()
        {
            return repository.Get(i => i.Status.ToLower() != "new" && i.Status.ToLower() != "failed" 
            && i.Status.ToLower() != "updated", null, "").Count();
        }

        public int GetReceivedCount()
        {
            return repository.Get(i => i.IsReceiver == true, null, "").Count();
        }
        public DashboardDTO GetMonthlyDocuments(DateTime _date)
        {
            try
            {
                var Response = repository.Get(i => i.DateTimeIssued.Month == _date.Month && i.DateTimeIssued.Year == _date.Year, null, null).ToList();
                var _Receivedvaliddocs = Response.Where(i => i.Status.ToLower() == "valid" && i.IsReceiver != true);
                var _Receivedinvaliddocs = Response.Where(i => i.Status.ToLower() == "invalid" && i.IsReceiver != true);
                var _Receivedrejecteddocs = Response.Where(i => i.Status.ToLower() == "rejected" && i.IsReceiver != true);
                var _Receivedcancelleddocs = Response.Where(i => i.Status.ToLower() == "cancelled" && i.IsReceiver != true);
                var _Receivedsubmitteddocs = Response.Where(i => i.Status.ToLower() == "submitted" && i.IsReceiver != true);
                var ReceivedInvoices = _Receivedvaliddocs.Where(i => i.DocumentType.ToLower() == "i" && i.IsReceiver != true);
                var ReceivedCredits = _Receivedvaliddocs.Where(i => i.DocumentType.ToLower() == "c" && i.IsReceiver != true);
                var Receiveddebits = _Receivedvaliddocs.Where(i => i.DocumentType.ToLower() == "d" && i.IsReceiver != true);

                var _Submittedvaliddocs = Response.Where(i => i.Status.ToLower() == "valid" && i.IsReceiver == true);
                var _Submittedinvaliddocs = Response.Where(i => i.Status.ToLower() == "invalid" && i.IsReceiver == true);
                var _Submittedrejecteddocs = Response.Where(i => i.Status.ToLower() == "rejected" && i.IsReceiver == true);
                var _Submittedcancelleddocs = Response.Where(i => i.Status.ToLower() == "cancelled" && i.IsReceiver == true);
                var _Submittedsubmitteddocs = Response.Where(i => i.Status.ToLower() == "submitted" && i.IsReceiver == true);
                var SubmittedInvoices = _Submittedvaliddocs.Where(i => i.DocumentType.ToLower() == "i" && i.IsReceiver == true);
                var SubmittedCredits = _Submittedvaliddocs.Where(i => i.DocumentType.ToLower() == "c" && i.IsReceiver == true);
                var Submitteddebits = _Submittedvaliddocs.Where(i => i.DocumentType.ToLower() == "d" && i.IsReceiver == true);

                var response = new DashboardDTO()
                {
                    goodsModel = Response.Where(x => x.IsReceiver != true).SelectMany(b => b.InvoiceLines)?.Distinct().GroupBy(o => o.ItemCode).Select(x => new GoodsModel()
                    {
                        totalAmount = x.Sum(y => y.Total).ToString("N2"),
                        count = x.Sum(p => p.Quantity),
                        itemCode = x.Select(e => e.ItemCode).FirstOrDefault(),
                        itemDesc = x.Select(e => e.Description).FirstOrDefault(),
                        totalTax = x.Sum(c => c.TaxableItems.Sum(u => u.Amount)).ToString("N2")
                    }).OrderByDescending(x => x.count).ToList(),
                    receivedGoodsModel = Response.Where(x => x.IsReceiver == true).SelectMany(b => b.InvoiceLines)?.Distinct().GroupBy(o => o.ItemCode).Select(x => new GoodsModel()
                    {
                        totalAmount = x.Sum(y => y.Total).ToString("N2"),
                        count = x.Sum(p => p.Quantity),
                        itemCode = x.Select(e => e.ItemCode).FirstOrDefault(),
                        itemDesc = x.Select(e => e.Description).FirstOrDefault(),
                        totalTax = x.Sum(c => c.TaxableItems.Sum(u => u.Amount)).ToString("N2")
                    }).OrderByDescending(x => x.count).ToList(),
                    ReceivedInvoiceTotalAmount = ReceivedInvoices.Sum(x => Convert.ToDecimal(x.TotalAmount)).ToString("N2"),
                    ReceivedInvoiceCount = ReceivedInvoices.Count(),
                    //ReceivedInvoiceTotalTax = (ReceivedInvoices.Sum(x => Convert.ToDecimal(x.TotalSalesAmount)) - ReceivedInvoices.Sum(x => Convert.ToDecimal(x.NetAmount))).ToString("N2"),
                    ReceivedInvoiceTotalTax = ReceivedInvoices.SelectMany(o => o.InvoiceLines).SelectMany(i => i.TaxableItems).Sum(t => t.Amount).ToString("N2"),
                    ReceivedCreditTotalAmount = ReceivedCredits.Sum(x => Convert.ToDecimal(x.TotalAmount)).ToString("N2"),
                    ReceivedCreditCount = ReceivedCredits.Count(),
                    //ReceivedCreditTotalTax = (ReceivedCredits.Sum(x => Convert.ToDecimal(x.TotalSalesAmount)) - ReceivedCredits.Sum(x => Convert.ToDecimal(x.NetAmount))).ToString("N2"),
                    ReceivedCreditTotalTax = ReceivedCredits.SelectMany(o => o.InvoiceLines).SelectMany(i => i.TaxableItems).Sum(t => t.Amount).ToString("N2"),
                    ReceivedDebitTotalAmount = Receiveddebits.Sum(x => Convert.ToDecimal(x.TotalAmount)).ToString("N2"),
                    ReceivedDebitCount = Receiveddebits.Count(),
                    //ReceivedDebitTotalTax = (Receiveddebits.Sum(x => Convert.ToDecimal(x.TotalSalesAmount)) - Receiveddebits.Sum(x => Convert.ToDecimal(x.NetAmount))).ToString("N2"),
                    ReceivedDebitTotalTax = Receiveddebits.SelectMany(o => o.InvoiceLines).SelectMany(i => i.TaxableItems).Sum(t => t.Amount).ToString("N2"),
                    ReceivedValidDocumentsCount = _Receivedvaliddocs.Count(),
                    ReceivedCanceledDocumentsCount = _Receivedcancelleddocs.Count(),
                    ReceivedRejectedDocumentsCount = _Receivedrejecteddocs.Count(),
                    ReceivedInValidDocumentsCount = _Receivedinvaliddocs.Count(),
                    ReceivedSubmittedDocumentsCount = _Receivedsubmitteddocs.Count(),


                    SubmittedInvoiceTotalAmount = SubmittedInvoices.Sum(x => Convert.ToDecimal(x.TotalAmount)).ToString("N2"),
                    SubmittedInvoiceCount = SubmittedInvoices.Count(),
                    //SubmittedInvoiceTotalTax = (SubmittedInvoices.Sum(x => Convert.ToDecimal(x.TotalSalesAmount)) - SubmittedInvoices.Sum(x => Convert.ToDecimal(x.NetAmount))).ToString("N2"),
                    SubmittedInvoiceTotalTax = SubmittedInvoices.SelectMany(o => o.InvoiceLines).SelectMany(i => i.TaxableItems).Sum(t => t.Amount).ToString("N2"),
                    SubmittedCreditTotalAmount = SubmittedCredits.Sum(x => Convert.ToDecimal(x.TotalAmount)).ToString("N2"),
                    SubmittedCreditCount = SubmittedCredits.Count(),
                    //SubmittedCreditTotalTax = (SubmittedCredits.Sum(x => Convert.ToDecimal(x.TotalSalesAmount)) - SubmittedCredits.Sum(x => Convert.ToDecimal(x.NetAmount))).ToString("N2"),
                    SubmittedCreditTotalTax = SubmittedCredits.SelectMany(o => o.InvoiceLines).SelectMany(i => i.TaxableItems).Sum(t => t.Amount).ToString("N2"),
                    SubmittedDebitTotalAmount = Submitteddebits.Sum(x => Convert.ToDecimal(x.TotalAmount)).ToString("N2"),
                    SubmittedDebitCount = Submitteddebits.Count(),
                    //SubmittedDebitTotalTax = (Submitteddebits.Sum(x => Convert.ToDecimal(x.TotalSalesAmount)) - Submitteddebits.Sum(x => Convert.ToDecimal(x.NetAmount))).ToString("N2"),
                    SubmittedDebitTotalTax = Submitteddebits.SelectMany(o => o.InvoiceLines).SelectMany(i => i.TaxableItems).Sum(t => t.Amount).ToString("N2"),
                    SubmittedValidDocumentsCount = _Submittedvaliddocs.Count(),
                    SubmittedCanceledDocumentsCount = _Submittedcancelleddocs.Count(),
                    SubmittedRejectedDocumentsCount = _Submittedrejecteddocs.Count(),
                    SubmittedInValidDocumentsCount = _Submittedinvaliddocs.Count(),
                    SubmittedDocumentsCount = _Submittedsubmitteddocs.Count()
                };
                response.ReceivedDocumentsCount = response.ReceivedValidDocumentsCount + response.ReceivedInValidDocumentsCount 
                    + response.ReceivedCanceledDocumentsCount + response.ReceivedRejectedDocumentsCount + response.ReceivedSubmittedDocumentsCount;

                response.allSubmittedDocumentsCount = response.SubmittedValidDocumentsCount + response.SubmittedInValidDocumentsCount
                + response.SubmittedCanceledDocumentsCount + response.SubmittedRejectedDocumentsCount + response.SubmittedDocumentsCount;

                if (response.ReceivedDocumentsCount != 0)
                {
                    response.ReceivedValidDocumentsCountPercentage = Math.Round((response.ReceivedValidDocumentsCount * 100.00) / response.ReceivedDocumentsCount, 2);
                    response.ReceivedInValidDocumentsCountPercentage = Math.Round((response.ReceivedInValidDocumentsCount * 100.00) / response.ReceivedDocumentsCount, 2);
                    response.ReceivedRejectedDocumentsCountPercentage = Math.Round((response.ReceivedRejectedDocumentsCount * 100.00) / response.ReceivedDocumentsCount, 2);
                    response.ReceivedCanceledDocumentsCountPercentage = Math.Round((response.ReceivedCanceledDocumentsCount * 100.00) / response.ReceivedDocumentsCount, 2);
                    response.ReceivedSubmittedDocumentsCountPercentage = Math.Round((response.ReceivedSubmittedDocumentsCount * 100.00) / response.ReceivedDocumentsCount, 2);
                }

                if (response.allSubmittedDocumentsCount != 0)
                {
                    response.SubmittedValidDocumentsCountPercentage = Math.Round((response.SubmittedValidDocumentsCount * 100.00) / response.allSubmittedDocumentsCount, 2);
                    response.SubmittedInValidDocumentsCountPercentage = Math.Round((response.SubmittedInValidDocumentsCount * 100.00) / response.allSubmittedDocumentsCount, 2);
                    response.SubmittedRejectedDocumentsCountPercentage = Math.Round((response.SubmittedRejectedDocumentsCount * 100.00) / response.allSubmittedDocumentsCount, 2);
                    response.SubmittedCanceledDocumentsCountPercentage = Math.Round((response.SubmittedCanceledDocumentsCount * 100.00) / response.allSubmittedDocumentsCount, 2);
                    response.SubmittedDocumentsCountPercentage = Math.Round((response.SubmittedDocumentsCount * 100.00) / response.allSubmittedDocumentsCount, 2);
                }
                return response;
            }
            catch
            {
                return new DashboardDTO();
            }
        }
        public PagedList<DocumentVM> GetSubmittedDocuments(int pageNumber, int pageSize, DateTime fromDate,
            DateTime toDate, string searchValue, string sortColumnName, string sortDirection, string status)
        {
            toDate = toDate.AddDays(1);
            IQueryable<Document> docs;
            if (string.IsNullOrEmpty(status) || status.ToLower() == "all")
            {
                docs = repository.Get(i => i.Status.ToLower() != "new" && i.Status.ToLower() != "failed" && i.Status.ToLower() != "updated" && i.IsReceiver != true
                && (i.DateTimeReceived >= fromDate.Date && i.DateTimeReceived <= toDate.Date), m => m.OrderByDescending(x => x.DateTimeReceived), "Errors");
            }
            else
            {
                docs = repository.Get(i => i.Status.ToLower() == status.ToLower() && i.IsReceiver != true
                && (i.DateTimeReceived >= fromDate.Date && i.DateTimeReceived <= toDate.Date), m => m.OrderByDescending(x => x.DateTimeReceived), "");
            }
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                searchValue = searchValue.ToLower().Replace("/", "");
                docs = docs.Where(x => x.Id.ToString().Contains(searchValue) || x.Status.ToString().ToLower().Contains(searchValue) ||
                DbFunctions.TruncateTime(x.DateTimeReceived).ToString().Replace("-", "").Contains(searchValue) || DbFunctions.TruncateTime(x.DateTimeIssued).ToString().Replace("-", "").Contains(searchValue) || 
                searchValue.Contains(x.DocumentType.ToLower()) || x.DocumentTypeVersion.ToLower().Contains(searchValue) || x.TotalSalesAmount.ToString().ToLower().Contains(searchValue) ||
                x.TotalItemsDiscountAmount.ToString().ToLower().Contains(searchValue) || x.TotalAmount.ToString().ToLower().Contains(searchValue) ||
                x.ReceiverName.ToLower().Contains(searchValue) || x.ReceiverType.ToLower().Contains(searchValue));
            }
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                docs = docs.OrderBy(sortColumnName + " " + sortDirection);
            }
            var temp = PagedList<Document>.Create(docs, pageNumber, pageSize, docs.Count());
            return new PagedList<DocumentVM>(temp.Select(x => x.ToDocumentVM()).ToList(), docs.Count(), pageNumber, pageSize, docs.Count());
        }

        public PagedList<DocumentVM> GetReceivedDocuments(int pageNumber, int pageSize, DateTime fromDate,
            DateTime toDate, string searchValue, string sortColumnName, string sortDirection, string status)
        {
            toDate = toDate.AddDays(1);
            IQueryable<Document> docs;
            if (string.IsNullOrEmpty(status) || status.ToLower() == "all")
            {
                docs = repository.Get(i => i.Status.ToLower() != "new" && i.Status.ToLower() != "failed" && i.Status.ToLower() != "updated" && i.IsReceiver == true
                && (i.DateTimeReceived >= fromDate.Date && i.DateTimeReceived <= toDate.Date), m => m.OrderByDescending(x => x.DateTimeReceived), "");
            }
            else
            {
                docs = repository.Get(i => i.Status.ToLower() == status.ToLower() && i.IsReceiver == true
                && (i.DateTimeReceived >= fromDate.Date && i.DateTimeReceived <= toDate.Date), m => m.OrderByDescending(x => x.DateTimeReceived), "");
            }
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                searchValue = searchValue.ToLower().Replace("/", "");
                docs = docs.Where(x => x.Id.ToString().Contains(searchValue) || x.Status.ToString().ToLower().Contains(searchValue) ||
                DbFunctions.TruncateTime(x.DateTimeReceived).ToString().Replace("-", "").Contains(searchValue) || DbFunctions.TruncateTime(x.DateTimeIssued).ToString().Replace("-", "").Contains(searchValue) ||
                searchValue.Contains(x.DocumentType.ToLower()) || x.DocumentTypeVersion.ToLower().Contains(searchValue) || x.TotalSalesAmount.ToString().ToLower().Contains(searchValue) ||
                x.TotalItemsDiscountAmount.ToString().ToLower().Contains(searchValue) || x.TotalAmount.ToString().ToLower().Contains(searchValue) ||
                x.ReceiverName.ToLower().Contains(searchValue) || x.ReceiverType.ToLower().Contains(searchValue));
            }
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                docs = docs.OrderBy(sortColumnName + " " + sortDirection);
            }
            var temp = PagedList<Document>.Create(docs, pageNumber, pageSize, docs.Count());
            return new PagedList<DocumentVM>(temp.Select(x => x.ToDocumentVM()).ToList(), docs.Count(), pageNumber, pageSize, docs.Count());
        }
        public DocumentVM GetDocumentById(string Id)
        {
            return repository.GetAllIncluding(i => i.Id == Id, null, "InvoiceLines, Errors").ToList().Select(e => e.ToDocumentVM()).FirstOrDefault();
        }

        public NewDocumentVM GetDocumentByInternalId(string InternalId)
        {
            var result = repository.GetAllIncluding(i => i.Id == InternalId, null, "InvoiceLines").FirstOrDefault();
            return AutoMapperConfiguration.Mapper.Map<NewDocumentVM>(result);
        }
        public DocumentVM GetDocumentByuuid(string uuid)
        {
            return repository.GetAllIncluding(i => i.uuid == uuid, null, "InvoiceLines").Select(e => e.ToDocumentVM()).FirstOrDefault();
        }
        public void UpdateDocuments(DocumentSubmissionDTO obj, string submittedBy)
        {
            if (obj.acceptedDocuments != null)
            {
                foreach (var item in obj.acceptedDocuments)
                {
                    var entity = repository.Get(item.internalId);
                    entity.Status = "Submitted";
                    entity.DateTimeReceived = DateTime.Now;
                    entity.submissionId = obj.submissionId;
                    entity.uuid = item.uuid;
                    entity.longId = item.longId;
                    entity.SubmittedBy = submittedBy;
                    repository.UpdateBulk(entity);
                }
            }
            if (obj.rejectedDocuments != null)
            {
                foreach (var item in obj.rejectedDocuments)
                {
                    var entity = repository.Get(item.internalId);
                    entity.Status = "Failed";
                    entity.SubmittedBy = submittedBy;
                    repository.UpdateBulk(entity);
                }
            }
        }
        private void UpdateDocumentsStatus(GetRecentDocumentsResponse obj)
        {
            try
            {
                foreach (var item in obj.result)
                {
                    if (!IdsStack.Contains(item.internalId))
                    {
                        var entity = repository.Get(item.internalId);
                        if (entity != null && ((entity.Status.ToLower() == "valid" && item.status.ToLower() != "invalid") 
                            || (entity.Status.ToLower() == "submitted" && item.status.ToLower() == "invalid") || entity.Status.ToLower() == "submitted" || 
                            item.status.ToLower() == "rejected" || item.status.ToLower() == "cancelled"))
                        {
                            entity.Status = item.status;
                            entity.uuid = item.uuid;
                            entity.submissionId = item.submissionUUID;
                            entity.longId = item.longId;
                            entity.DateTimeReceived = DateTime.Parse(item.dateTimeReceived);
                            repository.UpdateBulk(entity);
                            IdsStack.Add(item.internalId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool RequestDocumentCancellation(string uuid, string reason)
        {
            var entity = repository.GetDocumentByuuid(uuid);
            if (entity != null && entity.Status.ToLower() == "valid")
            {
                entity.IsCancelRequested = true;
                entity.CancelRequestDate = DateTime.Now;
                entity.DocumentStatusReason = reason;
                var res = repository.Update(entity);
                if (res != null)
                    return true;
                else
                    return false;
            }
            return false;
        }
        // Create document through this portal
        public void CreateNewDocument(NewDocumentVM obj)
        {
            try
            {
                obj.Id = Guid.NewGuid().ToString();
                repository.Add(AutoMapperConfiguration.Mapper.Map<Document>(obj));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsertReceivedDocuments(GetRecentDocumentsResponse obj, string key, string url)
        {
            try
            {
                foreach (var item in obj.result)
                {
                    if (!IdsStack2.Contains(item.internalId))
                    {
                        var entity = repository.Get(item.internalId);
                        if (entity == null)
                        {
                            var result = GetDocument_ETA(url, key, item.uuid);
                            if (result != null)
                            {
                                result.isReceiver = true;
                                var res = repository.Add(result.ToDocument());
                                if (res != null)
                                {
                                    IdsStack2.Add(item.internalId);
                                }
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

        public void CreateNewDocumentWithOldId(NewDocumentVM obj)
        {
            try
            {
                repository.Add(AutoMapperConfiguration.Mapper.Map<Document>(obj));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EditDocument(NewDocumentVM obj)
        {
            try
            {
                var entity = repository.Get(obj.Id);
                entity = AutoMapperConfiguration.Mapper.Map<Document>(obj);
                var res = repository.Update(entity);
                if (res != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteDocument(string Id)
        {
            try
            {
                var response = _validationStepRepository.GetAllIncluding(v => v.DocumentId == Id).Select(d => d.Id).ToList();
                if (response.Count() > 0)
                {
                    _stepErrorRepository.DeleteByValidationStepId(response);
                }
                var entity = repository.Delete(Id);
                if (entity != null)
                    return true;
                else
                    return false;

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetInvalidDocumentsReasone(string BaseURL, string Key)
        {
            try
            {
                var InvalidDocuments = repository.Get(i => (i.Status.ToLower() == "invalid"), null).Select(x => new { UUID = x.uuid, ID = x.Id }).ToList();
                for (int i = 0; i < InvalidDocuments.Count(); i++)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string URL = BaseURL + "documents/" + InvalidDocuments[i].UUID + "/details";
                        client.BaseAddress = new Uri(URL);
                        var postTask = client.GetAsync(URL);
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var response = JsonConvert.DeserializeObject<GetDocumentResponse>(result.Content.ReadAsStringAsync().Result);
                            AddorUpdateInvalidReasons(response);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void AddorUpdateInvalidReasons(GetDocumentResponse obj)
        {
            if (!_validationStepRepository.CheckReasonsInsertedBefore(obj.internalId))
            {
                var res = _validationStepRepository.AddRange(obj.ToValidationSteps());
                Debug.WriteLine(res);
            }
            // first time will be in valid so we will ask the customer to modify then while syncing delete everything related to this document from validationsteps table.
        }

        public void UpdateDocumentByInternalId(string InternalId)
        {
            if (InternalId != null)
            {
                var entity = repository.Get(InternalId);
                entity.Status = "New";
                entity.submissionId = null;
                entity.uuid = null;
                entity.longId = null;
                entity.SubmittedBy = null;
                repository.Update(entity);
            }
        }
    }
}
