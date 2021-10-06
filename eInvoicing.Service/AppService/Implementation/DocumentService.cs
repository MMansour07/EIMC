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

namespace eInvoicing.Service.AppService.Implementation
{
    public class DocumentService :  IDocumentService
    {
        private readonly IDocumentRepository repository;
        public DocumentService(IDocumentRepository _repository)
        {
            this.repository = _repository;
        }
        public Task<HttpContent> GetDocumentPrintOut(string URL, string Key, string uuid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                URL += "documents/" + uuid + "/pdf";
                client.BaseAddress = new Uri(URL);
                var postTask = client.GetAsync(URL);
                postTask.Wait();
                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                { 
                    return Task.FromResult(result.Content);
                }
            }
                    
            return null;
        }
        public void GetRecentDocuments_ETA(string URL, string Key, int DailyInvoicesAverage)
        {
            try
            {
                for (int i = 0; i < DailyInvoicesAverage/100; i++)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Clear();
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
            catch
            {

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
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "documents/"+uuid+"/raw";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var GetDocumentResponseJSON = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                        var document = GetDocumentResponseJSON.SelectToken("document");
                        var documentProps = JObject.Parse(document.ToString()).Properties();
                        documentProps.Where(attr => attr.Name.Equals("taxAuthorityDocument")).ToList().ForEach(attr => attr.Remove());
                        GetDocumentResponseJSON.Properties().Where(attr => attr.Name.Equals("document")).ToList().ForEach(attr => attr.Remove());
                        var documentJsonStr = JsonConvert.SerializeObject(new JObject(documentProps));
                        GetDocumentResponseJSON.Add("document", JObject.Parse(documentJsonStr));
                        var response = JsonConvert.DeserializeObject<GetDocumentResponse>(GetDocumentResponseJSON.ToString());
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                        return response;
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
        public int CancelDocument(string URL, string Key, string uuid,string reason)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    URL += "documents/"+uuid+"/state";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PutAsJsonAsync(URL, new
                    {
                        status = "cancelled",
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
                return 0 ;
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
            return repository.Get(i => i.Status.ToLower() == "new" || i.Status.ToLower() == "updated", m => m.OrderBy(x => x.DateTimeIssued), "InvoiceLines").ToList().Select(x => x.ToDocumentVM());
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
            return repository.Get(i => i.Status.ToLower() == "new" || i.Status.ToLower() == "failed" || i.Status.ToLower() == "updated", m => m.OrderByDescending(x => x.DateTimeIssued), "InvoiceLines").Count();
        }
        public int GetSubmittedCount()
        {
            return repository.Get(i => i.Status.ToLower() != "new" && i.Status.ToLower() != "failed" && i.Status.ToLower() != "updated", m => m.OrderByDescending(x => x.DateTimeIssued), "InvoiceLines,Errors").Count();
        }
        public DashboardDTO GetMonthlyDocuments(DateTime _date)
        {
            try
            {
                var Response = repository.Get(i => i.DateTimeIssued.Month == _date.Month && i.DateTimeIssued.Year == _date.Year, null, null).ToList();
                var _Receivedvaliddocs = Response.Where(i => i.Status.ToLower() == "valid" );
                var _Receivedinvaliddocs = Response.Where(i => i.Status.ToLower() == "invalid");
                var _Receivedrejecteddocs = Response.Where(i => i.Status.ToLower() == "rejected");
                var _Receivedcancelleddocs = Response.Where(i => i.Status.ToLower() == "cancelled");
                var _Receivedsubmitteddocs = Response.Where(i => i.Status.ToLower() == "submitted");
                var ReceivedInvoices = _Receivedvaliddocs.Where(i => i.DocumentType.ToLower() == "i");
                var ReceivedCredits = _Receivedvaliddocs.Where(i => i.DocumentType.ToLower() == "c");
                var Receiveddebits = _Receivedvaliddocs.Where(i => i.DocumentType.ToLower() == "d");
                var response = new DashboardDTO()
                {
                    goodsModel = Response.SelectMany(b => b.InvoiceLines)?.Distinct().GroupBy(o => o.ItemCode).Select(x => new GoodsModel() { totalAmount = x.Sum(y => y.Total).ToString("N2"),count = x.Sum(p => p.Quantity), itemCode = x.Select(e => e.ItemCode).FirstOrDefault(), itemDesc = x.Select(e => e.Description).FirstOrDefault(), totalTax = x.Sum(c => c.TaxableItems.Sum(u => u.Amount)).ToString("N0") }).OrderByDescending(x => x.count).ToList(),
                    ReceivedInvoiceTotalAmount = ReceivedInvoices.Sum(x => Convert.ToDecimal(x.TotalAmount)).ToString("N1"),
                    ReceivedInvoiceCount = ReceivedInvoices.Count(),
                    ReceivedInvoiceTotalTax = (ReceivedInvoices.Sum(x => Convert.ToDecimal(x.TotalSalesAmount)) - ReceivedInvoices.Sum(x => Convert.ToDecimal(x.NetAmount))).ToString("N2"),
                    ReceivedCreditTotalAmount = ReceivedCredits.Sum(x => Convert.ToDecimal(x.TotalAmount)).ToString("N1"),
                    ReceivedCreditCount = ReceivedCredits.Count(),
                    ReceivedCreditTotalTax = (ReceivedCredits.Sum(x => Convert.ToDecimal(x.TotalSalesAmount)) - ReceivedCredits.Sum(x => Convert.ToDecimal(x.NetAmount))).ToString("N2"),
                    ReceivedDebitTotalAmount = Receiveddebits.Sum(x => Convert.ToDecimal(x.TotalAmount)).ToString("N1"),
                    ReceivedDebitCount = Receiveddebits.Count(),
                    ReceivedDebitTotalTax = (Receiveddebits.Sum(x => Convert.ToDecimal(x.TotalSalesAmount)) - Receiveddebits.Sum(x => Convert.ToDecimal(x.NetAmount))).ToString("N2"),
                    ReceivedValidDocumentsCount = _Receivedvaliddocs.Count(),
                    ReceivedCanceledDocumentsCount = _Receivedcancelleddocs.Count(),
                    ReceivedRejectedDocumentsCount = _Receivedrejecteddocs.Count(),
                    ReceivedInValidDocumentsCount = _Receivedinvaliddocs.Count(),
                    ReceivedSubmittedDocumentsCount = _Receivedsubmitteddocs.Count()
                };
                response.ReceivedDocumentsCount = response.ReceivedValidDocumentsCount + response.ReceivedInValidDocumentsCount + response.ReceivedCanceledDocumentsCount + response.ReceivedRejectedDocumentsCount + response.ReceivedSubmittedDocumentsCount;
                if (response.ReceivedDocumentsCount != 0)
                {
                    response.ReceivedValidDocumentsCountPercentage = Math.Round((response.ReceivedValidDocumentsCount * 100.00) / response.ReceivedDocumentsCount, 2);
                    response.ReceivedInValidDocumentsCountPercentage = Math.Round((response.ReceivedInValidDocumentsCount * 100.00) / response.ReceivedDocumentsCount, 2);
                    response.ReceivedRejectedDocumentsCountPercentage = Math.Round((response.ReceivedRejectedDocumentsCount * 100.00) / response.ReceivedDocumentsCount, 2);
                    response.ReceivedCanceledDocumentsCountPercentage = Math.Round((response.ReceivedCanceledDocumentsCount * 100.00) / response.ReceivedDocumentsCount, 2);
                    response.ReceivedSubmittedDocumentsCountPercentage = Math.Round((response.ReceivedSubmittedDocumentsCount * 100.00) / response.ReceivedDocumentsCount, 2);
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
                docs = repository.Get(i => i.Status.ToLower() != "new" && i.Status.ToLower() != "failed" && i.Status.ToLower() != "updated" 
                && (i.DateTimeReceived >= fromDate.Date && i.DateTimeReceived <= toDate.Date), m => m.OrderByDescending(x => x.DateTimeReceived), "InvoiceLines,Errors");
            }
            else
            {
                docs = repository.Get(i => i.Status.ToLower() == status.ToLower() 
                && (i.DateTimeReceived >= fromDate.Date && i.DateTimeReceived <= toDate.Date), m => m.OrderByDescending(x => x.DateTimeReceived), "InvoiceLines");
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
            foreach (var item in obj.result)
            {
                var entity = repository.Get(item.internalId);
                if (entity != null && entity.Status.ToLower() != "valid")
                {
                    entity.Status = item.status;
                    entity.uuid = item.uuid;
                    entity.submissionId = item.submissionUUID;
                    entity.longId = item.longId;
                    entity.DateTimeReceived = DateTime.Parse(item.dateTimeReceived);
                    repository.UpdateBulk(entity);
                }
            }
        }
    }
}
