using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using eInvoicing.Repository.Contract;
using System.Linq.Dynamic;
using System.Data.Entity.SqlServer;

namespace eInvoicing.Service.AppService.Implementation
{
    public class ReportService :  IReportService
    {
        private readonly IDocumentRepository repository;
        public ReportService(IDocumentRepository _repository)
        {
            this.repository = _repository;
        }
        public PagedList<SubmittedDocumentsDTO> GetSubmittedDocumentsStats(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection)
        {
            toDate = toDate.AddDays(1);
            var docs = repository.Get(i => i.Status.ToLower() != "new" && i.Status.ToLower() != "failed" && i.Status.ToLower() != "updated" && 
            i.DateTimeReceived >= fromDate.Date && i.DateTimeReceived <= toDate.Date, null, null);
            var result = docs.GroupBy(o => o.DateTimeReceived.Day).Select(x => new SubmittedDocumentsDTO()
            {
                validCount = x.Where(p => p.Status.ToLower() == "valid").Count(),
                invalidCount = x.Where(p => p.Status.ToLower() == "invalid").Count(),
                cancelledCount = x.Where(p => p.Status.ToLower() == "cancelled").Count(),
                submittedCount = x.Where(p => p.Status.ToLower() == "submitted").Count(),
                rejectedCount = x.Where(p => p.Status.ToLower() == "rejected").Count(),
                submittedOn = x.Select(i => i.DateTimeReceived).FirstOrDefault(),
                issuedOn = x.Select(i => i.DateTimeIssued).FirstOrDefault(),
                submittedBy = x.Select(i => i.SubmittedBy).FirstOrDefault(),
                totalCount = x.Count()
            });
            int totalCount = result.Count();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                result = result.Where(x => x.issuedOn.ToString().ToLower().Contains(searchValue.ToLower()) || x.cancelledCount.ToString().ToLower().Contains(searchValue.ToLower()) || x.invalidCount.ToString().ToLower().Contains(searchValue.ToLower()) ||
                                x.rejectedCount.ToString().ToLower().Contains(searchValue.ToLower()) || x.submittedBy.ToString().ToLower().Contains(searchValue.ToLower()) || x.submittedCount.ToString().ToLower().Contains(searchValue.ToLower()) ||
                                x.submittedOn.ToString().ToLower().Contains(searchValue.ToLower()) || x.totalCount.ToString().ToLower().Contains(searchValue.ToLower()) || x.validCount.ToString().ToLower().Contains(searchValue.ToLower()));
            }
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                result = result.OrderBy(sortColumnName + " " + sortDirection);
            }
            return PagedList<SubmittedDocumentsDTO>.Create(result, pageNumber, pageSize, totalCount);
        }

        public IEnumerable<GoodsModel> GetMonthlyBestSeller(int SpecificDate)
        {
            var docs = repository.Get(i => i.DateTimeIssued.Year == SpecificDate, m => m.OrderByDescending(x => x.DateTimeIssued), null).ToList();
            return docs.GroupBy(o => o.DateTimeIssued.Month).Select(i => i.SelectMany(o => o.InvoiceLines).GroupBy(o => o.ItemCode).Select(x => new GoodsModel() 
            {
                totalAmount = x.Sum(y => y.Total).ToString("N0"), 
                count = x.Sum(y => y.Quantity), 
                itemCode = x.Select(e => e.ItemCode).FirstOrDefault(), 
                itemDesc = x.Select(e => e.Description).FirstOrDefault(), 
                totalTax = x.Sum(c => c.TaxableItems.Sum(u => u.Amount)).ToString("N0"),
                month = i.Select(t => t.DateTimeIssued.Month).FirstOrDefault()
            }).OrderByDescending(x => x.count).FirstOrDefault());
        }

        public IEnumerable<GoodsModel> GetMonthlyLowestSeller(int SpecificDate)
        {


            var docs = repository.Get(i => i.DateTimeIssued.Year == SpecificDate, m => m.OrderByDescending(x => x.DateTimeIssued), null).ToList();
            return docs.GroupBy(o => o.DateTimeIssued.Month).Select(i => i.SelectMany(o => o.InvoiceLines).GroupBy(o => o.ItemCode).Select(x => new GoodsModel()
            {
                totalAmount = x.Sum(y => y.Total).ToString("N0"),
                count = x.Sum(y => y.Quantity),
                itemCode = x.Select(e => e.ItemCode).FirstOrDefault(),
                itemDesc = x.Select(e => e.Description).FirstOrDefault(),
                totalTax = x.Sum(c => c.TaxableItems.Sum(u => u.Amount)).ToString("N0"),
                month = i.Select(t => t.DateTimeIssued.Month).FirstOrDefault()
            }).OrderBy(x => x.count).FirstOrDefault());
        }
        public PagedList<GoodsModel> GetTopGoodsUsage(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection)
        {
            toDate = toDate.AddDays(1);
            var Response = repository.Get(i => i.Status.ToLower() != "new" && i.Status.ToLower() != "failed" && i.Status.ToLower() != "updated" &&
             i.DateTimeReceived >= fromDate.Date && i.DateTimeReceived <= toDate.Date, null, null);
            var goodsModel = Response.SelectMany(b => b.InvoiceLines)?.Distinct().GroupBy(o => o.ItemCode)
             .Select(x => new GoodsModel() { totalAmount = SqlFunctions.StringConvert(x.Sum(y => y.Total)), count = x.Sum(p => p.Quantity),
                 itemCode = x.Select(e => e.ItemCode).FirstOrDefault(), itemDesc = x.Select(e => e.Description).FirstOrDefault(), totalTax = SqlFunctions.StringConvert(x.Sum(c => c.TaxableItems.Sum(u => u.Amount)))});
            int totalCount = goodsModel.Count();
            if (!string.IsNullOrEmpty(searchValue))//filter
            {
                goodsModel = goodsModel.Where(x => x.itemCode.ToString().ToLower().Contains(searchValue.ToLower()) || x.itemDesc.ToString().ToLower().Contains(searchValue.ToLower()) ||
                                x.totalAmount.ToString().ToLower().Contains(searchValue.ToLower()) || x.totalTax.ToString().ToLower().Contains(searchValue.ToLower()) 
                                || x.count.ToString().ToLower().Contains(searchValue.ToLower()));
            }
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                goodsModel = goodsModel.OrderBy(sortColumnName + " " + sortDirection);
            }
            return PagedList<GoodsModel>.Create(goodsModel, pageNumber, pageSize, totalCount);
        }
    }
}

