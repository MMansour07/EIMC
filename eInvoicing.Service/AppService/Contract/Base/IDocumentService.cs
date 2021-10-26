using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface IDocumentService
    {
        void GetRecentDocuments_ETA(string URL, string Key, int DailyInvoicesAverage);
        GetRecentDocumentsResponse GetRecentDocuments_ETA2(string URL, string Key, int pageNo, int pageSize);
        GetDocumentResponse GetDocument_ETA(string URL, string Key, string uuid);
        Task<HttpContent> GetDocumentPrintOut(string URL, string Key, string uuid);
        int CancelDocument(string URL, string Key, string uuid, string reason);
        int RejectDocument(string URL, string Key, string uuid, string reason);
        int DeclineDocumentCancellation(string URL, string Key, string uuid);
        int DeclineDocumentRejection(string URL, string Key, string uuid);
        PagedList<DocumentVM> GetPendingDocuments(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection, string status);
        PagedList<DocumentVM> GetSubmittedDocuments(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection, string status);
        IEnumerable<DocumentVM> GetAllDocumentsToSubmit();
        int GetPendingCount();
        int GetSubmittedCount();
        DashboardDTO GetMonthlyDocuments(DateTime _date);
        DocumentVM GetDocumentById(string Id);
        DocumentVM GetDocumentByuuid(string uuid);
        void UpdateDocuments(DocumentSubmissionDTO obj, string submittedBy);
        void CreateNewDocument(NewDocumentVM obj);
    }
}
