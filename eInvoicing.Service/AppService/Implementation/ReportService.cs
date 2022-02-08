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
        private readonly IValidationStepRepository _validationStepRepository;
        public ReportService(IDocumentRepository _repository, IValidationStepRepository validationStepRepository)
        {
            this.repository = _repository;
            this._validationStepRepository = validationStepRepository;
        }
        public void GetTheConnectionString(string ConnectionString)
        {
            this.repository.GetTheConnectionString(ConnectionString);
            this._validationStepRepository.GetTheConnectionString(ConnectionString);
        }
        public PagedList<SubmittedDocumentsDTO> GetSubmittedDocumentsStats(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection)
        {
            toDate = toDate.AddDays(1);
            var docs = repository.Get(i => i.uuid != null && i.IsReceiver != true &&
            i.DateTimeIssued >= fromDate.Date && i.DateTimeIssued < toDate.Date, null, null);
            var result = docs.GroupBy(o => new { DateTimeReceived= o.DateTimeReceived.Day, DateTimeIssued = o.DateTimeIssued.Day}).Select(x => new SubmittedDocumentsDTO()
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
                totalAmount = x.Sum(y => y.Total).ToString("N5"),
                count = x.Sum(y => y.Quantity), 
                itemCode = x.Select(e => e.ItemCode).FirstOrDefault(), 
                itemDesc = x.Select(e => e.Description).FirstOrDefault(), 
                totalTax = x.Sum(c => c.TaxableItems.Sum(u => u.Amount)).ToString("N5"),
                month = i.Select(t => t.DateTimeIssued.Month).FirstOrDefault()
            }).OrderByDescending(x => x.count).FirstOrDefault());
        }
        public IEnumerable<GoodsModel> GetMonthlyLowestSeller(int SpecificDate)
        {


            var docs = repository.Get(i => i.DateTimeIssued.Year == SpecificDate, m => m.OrderByDescending(x => x.DateTimeIssued), null).ToList();
            return docs.GroupBy(o => o.DateTimeIssued.Month).Select(i => i.SelectMany(o => o.InvoiceLines).GroupBy(o => o.ItemCode).Select(x => new GoodsModel()
            {
                totalAmount = x.Sum(y => y.Total).ToString("N5"),
                count = x.Sum(y => y.Quantity),
                itemCode = x.Select(e => e.ItemCode).FirstOrDefault(),
                itemDesc = x.Select(e => e.Description).FirstOrDefault(),
                totalTax = x.Sum(c => c.TaxableItems.Sum(u => u.Amount)).ToString("N5"),
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
        public PagedList<InvalidDocumentsReasonsDTO> GetInvalidDocumentReasons(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection)
        {
            try
            {
                toDate = toDate.AddDays(1);
                var validationSteps = _validationStepRepository.Get(i => i.DateTimeReceived >= fromDate.Date && i.DateTimeReceived <= toDate.Date, null, "StepErrors.InnerError").AsEnumerable();
                var result = validationSteps.GroupBy(o => o.DocumentId).Select(x => new InvalidDocumentsReasonsDTO()
                {
                    DocumentId = x.Select(i => i.DocumentId).FirstOrDefault(),
                    DateTimeIssued = x.Select(i => i.DateTimeIssued).FirstOrDefault(),
                    DateTimeReceived = x.Select(i => i.DateTimeReceived).FirstOrDefault(),
                    TotalAmount = x.Select(i => i.TotalAmount).FirstOrDefault(),
                    TotalDiscountAmount = x.Select(i => i.TotalDiscountAmount).FirstOrDefault(),
                    TotalItemsDiscountAmount = x.Select(i => i.TotalItemsDiscountAmount).FirstOrDefault(),
                    NetAmount = x.Select(i => i.NetAmount).FirstOrDefault(),
                    TotalSalesAmount = x.Select(i => i.TotalSalesAmount).FirstOrDefault(),
                    ExtraDiscountAmount = x.Select(i => i.ExtraDiscountAmount).FirstOrDefault(),
                    ValidationSteps = string.Join(", ", x.Select(p => p.StepName?.ToString()).Distinct()),
                    //Temp = x.SelectMany(p => p.StepErrors.Select(o => o.Error)).ToList(),
                    Errors = string.Join(", ", x.SelectMany(p => p.StepErrors.Select(o => o.Error).Distinct()).Distinct()),
                    InnerErrors = string.Join(", ", x.SelectMany(p => p.StepErrors.SelectMany(o => o.InnerError.Select(z => z.Error).Distinct()).Distinct()).Distinct()),
                });
                int totalCount = validationSteps.Count();
                if (!string.IsNullOrEmpty(searchValue))//filter
                {
                    result = result.Where(x => x.ValidationSteps.ToLower().Contains(searchValue) || x.Errors.Contains(searchValue) || x.InnerErrors.Contains(searchValue));
                }
                if (!string.IsNullOrEmpty(sortColumnName))
                {
                    result = result.OrderBy(sortColumnName + " " + sortDirection);
                }
                return PagedList<InvalidDocumentsReasonsDTO>.Create(result, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}

