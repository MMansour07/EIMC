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
    public interface IReportService
    {
        PagedList<SubmittedDocumentsDTO> GetSubmittedDocumentsStats(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection);
        IEnumerable<GoodsModel> GetMonthlyBestSeller(int SpecificDate);
        IEnumerable<GoodsModel> GetMonthlyLowestSeller(int SpecificDate);
        PagedList<GoodsModel> GetTopGoodsUsage(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection);
    }
}
