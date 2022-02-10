using eInvoicing.API.Filters;
using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace eInvoicing.API.Controllers
{
    [JwtAuthentication]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet]
        [Route("api/report/submitteddocumentsstats")]
        public IHttpActionResult submitteddocumentsstats(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection)
        {
            try
            {
                _reportService.GetTheConnectionString(this.OnActionExecuting());
                var response = _reportService.GetSubmittedDocumentsStats(pageNumber, pageSize, fromDate, toDate, searchValue, sortColumnName, sortDirection);
                return Ok(new SubmittedDocumentResponse() { meta = new Meta() { page = response.CurrentPage, pages = response.TotalPages, perpage = response.PageSize, 
                    total = response.TotalCount, totalFiltered = response.TotalFiltered }, data = response });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/report/submitteddocumentsstatsoverview")]
        public IHttpActionResult submitteddocumentsstatsoverview(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection)
        {
            try
            {
                _reportService.GetTheConnectionString(this.OnActionExecuting());
                var response = _reportService.GetDocumentsStatsOverview(pageNumber, pageSize, fromDate, toDate, searchValue, sortColumnName, sortDirection);
                return Ok(new SubmittedDocumentResponse()
                {
                    meta = new Meta()
                    {
                        page = response.CurrentPage,
                        pages = response.TotalPages,
                        perpage = response.PageSize,
                        total = response.TotalCount,
                        totalFiltered = response.TotalFiltered
                    },
                    data = response
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/report/invalidreasons")]
        public IHttpActionResult invalidreasons(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection)
        {
            try
            {
                _reportService.GetTheConnectionString(this.OnActionExecuting());
                var response = _reportService.GetInvalidDocumentReasons(pageNumber, pageSize, fromDate, toDate, searchValue, sortColumnName, sortDirection);
                return Ok(new InvalidDocumentResponse()
                {
                    meta = new Meta()
                    {
                        page = response.CurrentPage,
                        pages = response.TotalPages,
                        perpage = response.PageSize,
                        total = response.TotalCount,
                        totalFiltered = response.TotalFiltered
                    },
                    data = response
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/report/TopGoodsUsage")]
        public IHttpActionResult GetTopGoodsUsage(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection)
        {
            try
            {
                _reportService.GetTheConnectionString(this.OnActionExecuting());
                var response = _reportService.GetTopGoodsUsage(pageNumber, pageSize, fromDate, toDate, searchValue, sortColumnName, sortDirection);
                return Ok(new GoodsModelVM() { meta = new Meta() { page = response.CurrentPage, pages = response.TotalPages, perpage = response.PageSize,
                    total = response.TotalCount, totalFiltered = response.TotalFiltered }, data = response });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/report/MonthlyBestSeller")]
        public IHttpActionResult GetMonthlyBestSeller(int SpecificDate)
        {
            try
            {
                _reportService.GetTheConnectionString(this.OnActionExecuting());
                return Ok(_reportService.GetMonthlyBestSeller(SpecificDate));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/report/MonthlyLowestSeller")]
        public IHttpActionResult GetMonthlyLowestSeller(int SpecificDate)
        {
            try
            {
                _reportService.GetTheConnectionString(this.OnActionExecuting());
                return Ok(_reportService.GetMonthlyLowestSeller(SpecificDate));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}