using eInvoicing.API.Filters;
using eInvoicing.API.Models;
using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using Newtonsoft.Json;
using ProductLicense;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace eInvoicing.API.Controllers
{
    public class DocumentController : BaseController
    {
        private readonly IDocumentService _documentService;
        private readonly IErrorService _errorService;
        private readonly IAuthService _auth;
        private readonly IUserSession _userSession;
        OleDbConnection Econ;
        SqlConnection con;
        string constr, Query;
        bool HasUpdatedRecords;
        bool IsInserted, IsUpdated = false;
        List<string> UpdatedDocumentsIds;
        List<string> NonExistingDocumentIds;
        List<string> UpdatedInvoiceLinesIds;
        List<string> InternalDocumentIds = new List<string>();
        List<string> InternalInvoiceLinesIds = new List<string>();

        public DocumentController(IDocumentService documentService, IAuthService auth, IUserSession userSession, IErrorService errorService)
        {
            _documentService = documentService;
            _auth = auth;
            _userSession = userSession;
            _errorService = errorService;
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("api/document/new")]
        public IHttpActionResult CreateNewDocument(NewDocumentVM obj)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                _documentService.CreateNewDocument(obj);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("api/document/edit")]
        public IHttpActionResult EditDocument(NewDocumentVM obj)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                var res = _documentService.DeleteDocument(obj.Id);
                if (res)
                {
                    _documentService.CreateNewDocumentWithOldId(obj);
                    if (res)
                        return Ok();
                    else
                        return InternalServerError();
                }
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/getdocumentbyinternalid")]
        public IHttpActionResult GetDocumentByInternalId(string InternalId)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                return Ok(_documentService.GetDocumentByInternalId(InternalId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/UpdateDocumentByInternalId")]
        public IHttpActionResult UpdateDocumentByInternalId(string InternalId)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                _documentService.UpdateDocumentByInternalId(InternalId);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/pending")]
        public IHttpActionResult Pending(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection, string status)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                var docs = _documentService.GetPendingDocuments(pageNumber, pageSize, fromDate, toDate, searchValue, sortColumnName, sortDirection, status);
                return Ok(new DocumentResponse() { meta = new Meta() { page = docs.CurrentPage, pages = docs.TotalPages, perpage = docs.PageSize, total = docs.TotalCount }, data = docs });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/pendingCount")]
        public IHttpActionResult PendingCount()
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                return Ok(_documentService.GetPendingCount());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/InvalidandFailedCount")]
        public IHttpActionResult InvalidandFailedCount()
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                return Ok(_documentService.InvalidandFailedCount());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/receivedCount")]
        public IHttpActionResult ReceivedCount()
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                return Ok(_documentService.GetReceivedCount());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/MonthlyDocuments")]
        public IHttpActionResult GetMonthlyDocuments(DateTime _date)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                return Ok(_documentService.GetMonthlyDocuments(_date));
            }
            catch (Exception ex)
            {
                return Ok(new DashboardDTO() { Reason  = ex.Message.ToString()});
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/submitted")]
        public IHttpActionResult Submitted(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection, string status)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                var docs = _documentService.GetSubmittedDocuments(pageNumber, pageSize, fromDate, toDate, searchValue, sortColumnName, sortDirection, status);
                return Ok(new DocumentResponse() { meta = new Meta() { page = docs.CurrentPage, pages = docs.TotalPages, perpage = docs.PageSize, total = docs.TotalCount }, data = docs });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/received")]
        public IHttpActionResult Received(int pageNumber, int pageSize, DateTime fromDate, 
            DateTime toDate, string searchValue, string sortColumnName, string sortDirection, string status)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                var docs = _documentService.GetReceivedDocuments(pageNumber, pageSize, fromDate, toDate, searchValue, sortColumnName, sortDirection, status);
                return Ok(new DocumentResponse() { meta = new Meta() { page = docs.CurrentPage, pages = docs.TotalPages, perpage = docs.PageSize, total = docs.TotalCount }, data = docs });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/submittedCount")]
        public IHttpActionResult submittedCount()
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                return Ok(_documentService.GetSubmittedCount());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/GetByDocumentId")]
        public IHttpActionResult GetByDocumentId(string id)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                var docs = _documentService.GetDocumentById(id);
                return Ok(docs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/GetDocumentByuuid")]
        public IHttpActionResult GetDocumentByuuid(string uuid)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                var docs = _documentService.GetDocumentByuuid(uuid);
                return Ok(docs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/recent")]
        public IHttpActionResult GetRecentDocuments_ETA(int pageNo, int pageSize)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                _userSession.GetBusinessGroupId(this.GetBusinessGroupId());
                var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                var result = _documentService.GetRecentDocuments_ETA2(_userSession.submissionurl, auth.access_token, pageNo, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/raw")]
        public IHttpActionResult Raw(string uuid)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                _userSession.GetBusinessGroupId(this.GetBusinessGroupId());
                var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                var result = _documentService.GetDocument_ETA(_userSession.submissionurl, auth.access_token, uuid);
                if (result != null && result.StatusCode == HttpStatusCode.OK)
                    return Ok(result);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/printout")]
        public HttpResponseMessage GetDocumentPrintOut(string uuid)
        {
            try 
            {
                _userSession.GetBusinessGroupId(this.GetBusinessGroupId());
                var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                var result = _documentService.GetDocumentPrintOut(_userSession.submissionurl, auth.access_token, uuid);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/canceldocument")]
        public IHttpActionResult CancelDocument(string uuid, string reason = "For Correction.")
        {
            _documentService.GetTheConnectionString(this.OnActionExecuting());
            _userSession.GetBusinessGroupId(this.GetBusinessGroupId());

            var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
            var result = _documentService.CancelDocument(_userSession.submissionurl, auth.access_token, uuid, reason);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }

        
        public IHttpActionResult RejectDocument(string uuid, string reason)
        {
            _documentService.GetTheConnectionString(this.OnActionExecuting());
            _userSession.GetBusinessGroupId(this.GetBusinessGroupId());

            var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
            var result = _documentService.RejectDocument(_userSession.submissionurl, auth.access_token, uuid, reason);
            return Ok(result);
        }

        
        public IHttpActionResult DeclineDocumentCancellation(string uuid)
        {
            _documentService.GetTheConnectionString(this.OnActionExecuting());
            _userSession.GetBusinessGroupId(this.GetBusinessGroupId());

            var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
            var result = _documentService.DeclineDocumentCancellation(_userSession.submissionurl, auth.access_token, uuid);
            return Ok(result);
        }

        
        public IHttpActionResult DeclineDocumentRejection(string uuid)
        {
            _documentService.GetTheConnectionString(this.OnActionExecuting());
            _userSession.GetBusinessGroupId(this.GetBusinessGroupId());

            var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
            var result = _documentService.DeclineDocumentRejection(_userSession.submissionurl, auth.access_token, uuid);
            return Ok(result);
        }

        [JwtAuthentication]
        [Route("api/document/updatedocuments")]
        public IHttpActionResult UpdateDocuments(DocumentSubmissionDTO obj, string submittedBy)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                _documentService.UpdateDocuments(obj, submittedBy);
                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void ExcelConn(string FilePath)
        {

            constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties="+ Convert.ToChar(34).ToString()+ @"Excel 12.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString(), FilePath);
            Econ = new OleDbConnection(constr);

        }
        private void connection()
        {
            con = new SqlConnection(this.OnActionExecuting());
        }
        private int InsertDocuments()
        {
            Query = string.Format("Select [DocumentType],[DocumentTypeVersion], [TaxpayerActivityCode], [DateTimeIssued], [InternalDocumentId], [TotalSalesAmount], " +
                "                         [TotalDiscountAmount], [TotalItemsDiscountAmount], " +
                                         "[ExtraDiscountAmount], [NetAmount], [TotalAmount], [IssuerId], [IssuerName], [IssuerType], [IssuerBranchId], [IssuerCountry], [IssuerGovernorate]," +
                                         "[IssuerRegionCity], [IssuerStreet], [IssuerBuildingNumber], [ReceiverId], [ReceiverName], [ReceiverType], [ReceiverCountry]," +
                                         "[ReceiverGovernorate], [ReceiverRegionCity], [ReceiverStreet], [ReceiverBuildingNumber], [GrossWeight], " +
                                         "[NetWeight], [InternalDocumentStatus], 0 as [IsInternallyCreated], 'New' as [NewStatus], #" + DateTime.Now +"# as [DateTimeReceived] FROM [{0}]",
                                         "Documents$");

            // select document by id
            string SQLQuery = "SELECT [Id] FROM DOCUMENTS ";

            SqlCommand command = new SqlCommand(SQLQuery, con);
            var output = command.ExecuteReader();
            while (output.Read())
            {
                InternalDocumentIds.Add(output[0].ToString());
            }
            output.Close();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);

            oda.Fill(ds);
            DataTable Exceldt = null;

            //var xyz = ds.Tables[0].AsEnumerable().ToList();

            var NewRows = ds.Tables[0].AsEnumerable().Where(row => !InternalDocumentIds.Any(id => id == row["InternalDocumentId"].ToString()) && 
            row["InternalDocumentStatus"].ToString().ToLower() == "completed"); //|| row["InternalDocumentStatus"].ToString().ToLower() == "updated"

            //var UpdatedRows = ds.Tables[0].AsEnumerable().Where(row => InternalDocumentIds.Any(id => id == row["InternalDocumentId"].ToString()) &&
            //row["InternalDocumentStatus"].ToString().ToLower() == "updated"); //|| row["InternalDocumentStatus"].ToString().ToLower() == "updated"

            HasUpdatedRecords = ds.Tables[0].AsEnumerable().Where(row => row["InternalDocumentStatus"].ToString().ToLower() == "updated").Any();

            if (NewRows.Any())
            {
                Exceldt = NewRows.CopyToDataTable();
            }

            //creating object of SqlBulkCopy  
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            //assigning Destination table name  
            objbulk.DestinationTableName = "DOCUMENTS";
            //Mapping Table column  
            objbulk.ColumnMappings.Add("DocumentType", "DocumentType");
            objbulk.ColumnMappings.Add("DocumentTypeVersion", "DocumentTypeVersion");
            objbulk.ColumnMappings.Add("DateTimeIssued", "DateTimeIssued");
            objbulk.ColumnMappings.Add("TaxpayerActivityCode", "TaxpayerActivityCode");
            objbulk.ColumnMappings.Add("InternalDocumentId", "Id");
            objbulk.ColumnMappings.Add("TotalSalesAmount", "TotalSalesAmount");
            objbulk.ColumnMappings.Add("TotalDiscountAmount", "TotalDiscountAmount");
            objbulk.ColumnMappings.Add("TotalItemsDiscountAmount", "TotalItemsDiscountAmount");
            objbulk.ColumnMappings.Add("ExtraDiscountAmount", "ExtraDiscountAmount");
            objbulk.ColumnMappings.Add("NetAmount", "NetAmount");
            objbulk.ColumnMappings.Add("TotalAmount", "TotalAmount");
            objbulk.ColumnMappings.Add("IssuerId", "IssuerId");
            objbulk.ColumnMappings.Add("IssuerName", "IssuerName");
            objbulk.ColumnMappings.Add("IssuerType", "IssuerType");
            objbulk.ColumnMappings.Add("IssuerBranchId", "IssuerBranchId");
            objbulk.ColumnMappings.Add("IssuerCountry", "IssuerCountry");
            objbulk.ColumnMappings.Add("IssuerGovernorate", "IssuerGovernorate");
            objbulk.ColumnMappings.Add("IssuerRegionCity", "IssuerRegionCity");
            objbulk.ColumnMappings.Add("IssuerStreet", "IssuerStreet");
            objbulk.ColumnMappings.Add("IssuerBuildingNumber", "IssuerBuildingNumber");
            objbulk.ColumnMappings.Add("ReceiverId", "ReceiverId");
            objbulk.ColumnMappings.Add("ReceiverGovernorate", "ReceiverGovernorate");
            objbulk.ColumnMappings.Add("ReceiverStreet", "ReceiverStreet");
            objbulk.ColumnMappings.Add("ReceiverRegionCity", "ReceiverRegionCity");
            objbulk.ColumnMappings.Add("ReceiverBuildingNumber", "ReceiverBuildingNumber");
            objbulk.ColumnMappings.Add("ReceiverName", "ReceiverName");
            objbulk.ColumnMappings.Add("ReceiverType", "ReceiverType");
            objbulk.ColumnMappings.Add("ReceiverCountry", "ReceiverCountry");
            objbulk.ColumnMappings.Add("GrossWeight", "GrossWeight");
            objbulk.ColumnMappings.Add("NetWeight", "NetWeight");
            objbulk.ColumnMappings.Add("NewStatus", "Status"); 
            objbulk.ColumnMappings.Add("DateTimeReceived", "DateTimeReceived");
            objbulk.ColumnMappings.Add("IsInternallyCreated", "IsInternallyCreated");


            //inserting Datatable Records to DataBase  
            //con.Open();
            if (Exceldt != null)
            {
                objbulk.WriteToServer(Exceldt);
                IsInserted = true;
                InternalDocumentIds.AddRange(ds.Tables[0].AsEnumerable().Select(row => row["InternalDocumentId"].ToString()));
                return Exceldt.Rows.Count;
            }
            return 0;
        }
        private int InsertInvoiceLine()
        {
            List<DataRow> Lines = new List<DataRow>();
            Query = string.Format("Select [InternalInvoiceLineId],[ItemType], [ItemCode], [UnitType], [InternalCode], [Quantity], [AmountEGP], [AmountSold], " +
                                  "[CurrencySold], [CurrencyExchangeRate], [SalesTotal], [DiscountRate], [DiscountAmount], [ItemsDiscount], [TotalTaxableFees], [ValueDifference]," +
                                  "[NetTotal], [Total], [Description], [InternalDocumentId] FROM [{0}]", "Invoice Lines$");
            
            string SQLQuery = "SELECT [Id] FROM INVOICELINES";
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand command = new SqlCommand(SQLQuery, con);
            var output = command.ExecuteReader();
            while (output.Read())
            {
                InternalInvoiceLinesIds.Add(output[0].ToString());
            }
            output.Close();
            //OleDbCommand Ecom = new OleDbCommand(Query, Econ);
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
            oda.Fill(ds);
            DataTable Exceldt = null;

            var NewRows = ds.Tables[0].AsEnumerable().Where(row => !InternalInvoiceLinesIds.Any(id => id == row["InternalInvoiceLineId"].ToString()) && 
            InternalDocumentIds.Any(id => id == row["InternalDocumentId"].ToString()));
            foreach (var currentRow in NewRows)
            {
                currentRow.BeginEdit();
                currentRow["AmountEGP"]= Math.Round(Convert.ToDouble(currentRow["AmountEGP"]), 5);
                currentRow["CurrencyExchangeRate"] =  Math.Round(Convert.ToDouble(currentRow["CurrencyExchangeRate"]), 5);
                //currentRow["AmountEGP"]  = Math.Round((Math.Round(Convert.ToDouble(currentRow["AmountSold"]), 5) * 
                //    Math.Round(Convert.ToDouble(currentRow["CurrencyExchangeRate"]), 5)), 5);
                //currentRow["AmountSold"] = Math.Round(Convert.ToDouble(currentRow["AmountSold"]), 5);
                currentRow["AmountSold"] = Convert.ToDouble(currentRow["CurrencyExchangeRate"])  != 0 ? 
                    Math.Round(Convert.ToDouble(currentRow["AmountEGP"]) / Convert.ToDouble(currentRow["CurrencyExchangeRate"]), 5) : 0;
                currentRow["SalesTotal"] = Math.Round(Convert.ToDouble(currentRow["SalesTotal"]), 5);
                currentRow["NetTotal"]   = Math.Round(Convert.ToDouble(currentRow["NetTotal"]), 5);
                currentRow["Total"]      = Math.Round(Convert.ToDouble(currentRow["Total"]), 5);
                currentRow.EndEdit();
                Lines.Add(currentRow);
            }
            if (Lines.Any())
            {
                Exceldt = Lines.CopyToDataTable();
                //IsInserted = true;
            }
            //creating object of SqlBulkCopy  
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            //assigning Destination table name  
            objbulk.DestinationTableName = "INVOICELINES";
            //Mapping Table column  
            objbulk.ColumnMappings.Add("InternalInvoiceLineId", "Id");
            objbulk.ColumnMappings.Add("ItemType", "ItemType");
            objbulk.ColumnMappings.Add("ItemCode", "ItemCode");
            objbulk.ColumnMappings.Add("UnitType", "UnitType");
            objbulk.ColumnMappings.Add("InternalCode", "InternalCode");
            objbulk.ColumnMappings.Add("Quantity", "Quantity");
            objbulk.ColumnMappings.Add("AmountEGP", "AmountEGP");
            objbulk.ColumnMappings.Add("AmountSold", "AmountSold");
            objbulk.ColumnMappings.Add("CurrencySold", "CurrencySold");
            objbulk.ColumnMappings.Add("CurrencyExchangeRate", "CurrencyExchangeRate");
            objbulk.ColumnMappings.Add("SalesTotal", "SalesTotal");
            objbulk.ColumnMappings.Add("DiscountRate", "DiscountRate");
            objbulk.ColumnMappings.Add("DiscountAmount", "DiscountAmount");
            objbulk.ColumnMappings.Add("ItemsDiscount", "ItemsDiscount");
            objbulk.ColumnMappings.Add("TotalTaxableFees", "TotalTaxableFees");
            objbulk.ColumnMappings.Add("ValueDifference", "ValueDifference");
            objbulk.ColumnMappings.Add("NetTotal", "NetTotal");
            objbulk.ColumnMappings.Add("Total", "Total");
            objbulk.ColumnMappings.Add("Description", "Description");
            objbulk.ColumnMappings.Add("InternalDocumentId", "DocumentId");
            if (Exceldt != null)
            {
                objbulk.WriteToServer(Exceldt);
                InternalInvoiceLinesIds.AddRange(ds.Tables[0].AsEnumerable().Select(row => row["InternalInvoiceLineId"].ToString()));
                IsInserted = true;
                return Exceldt.Rows.Count;
            }
            return 0;

        }
        private int InsertTaxableItems()
        {
            Query = string.Format("Select [InternalId],[TaxType], [Rate], [Amount], [SubType], [InternalInvoiceLineId] FROM [{0}]", "Taxable Items$");
            string SQLQuery = "SELECT  CONCAT(InternalId,InvoiceLineId) AS 'UniqueTaxableItemKey'  FROM TAXABLEITEMS";
            List<string> UniqueTaxableItemKeys = new List<string>();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand command = new SqlCommand(SQLQuery, con);
            var output = command.ExecuteReader();
            while (output.Read())
            {
                UniqueTaxableItemKeys.Add(output[0].ToString());
            }
            output.Close();
            //OleDbCommand Ecom = new OleDbCommand(Query, Econ);
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
            oda.Fill(ds);
            DataTable Exceldt = null;

            var NewRows = ds.Tables[0].AsEnumerable().Where(row => !UniqueTaxableItemKeys.Any(key => key == row["InternalId"].ToString() + row["InternalInvoiceLineId"].ToString()) && InternalInvoiceLinesIds.Any(key => key == row["InternalInvoiceLineId"].ToString()));
            if (NewRows.Any())
            {
                Exceldt = NewRows.CopyToDataTable();
                IsInserted = true;
            }
            //creating object of SqlBulkCopy  
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            //assigning Destination table name  
            objbulk.DestinationTableName = "TAXABLEITEMS";
            //Mapping Table column  
            objbulk.ColumnMappings.Add("InternalId", "InternalId");
            objbulk.ColumnMappings.Add("TaxType", "TaxType");
            objbulk.ColumnMappings.Add("Rate", "Rate");
            objbulk.ColumnMappings.Add("Amount", "Amount");
            objbulk.ColumnMappings.Add("SubType", "SubType");
            objbulk.ColumnMappings.Add("InternalInvoiceLineId", "InvoiceLineId");

            if (Exceldt != null)
            {
                objbulk.WriteToServer(Exceldt);

                return Exceldt.Rows.Count;
            }
            return 0;

        }
        private int UpdateDocument()
        {
            try
            {
                Query = string.Format("Select [DocumentType],[DocumentTypeVersion], [TaxpayerActivityCode], [DateTimeIssued], [InternalDocumentId], [TotalSalesAmount], [TotalDiscountAmount], " +
                                      "[TotalItemsDiscountAmount], [ExtraDiscountAmount], [NetAmount], [TotalAmount], [IssuerId], [IssuerName], [IssuerType], [IssuerBranchId], [IssuerCountry], [IssuerGovernorate]," +
                                      "[IssuerRegionCity], [IssuerStreet], [IssuerBuildingNumber], [ReceiverId], [ReceiverName], [ReceiverType], [ReceiverCountry]," +
                                      "[ReceiverGovernorate], [ReceiverRegionCity], [ReceiverStreet], [ReceiverBuildingNumber], [GrossWeight], [NetWeight], [InternalDocumentStatus], " +
                                      "'New' as [NewStatus], #" + DateTime.Now + "# as [DateTimeReceived] FROM [{0}]", "Documents$");

                string SQLQuery = "SELECT [Id] FROM DOCUMENTS";
                List<string> InternalDocumentIds = new List<string>();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand(SQLQuery, con);
                var output = command.ExecuteReader();
                while (output.Read())
                {
                    InternalDocumentIds.Add(output[0].ToString());
                }
                output.Close();
                DataSet ds = new DataSet();
                OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
                oda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var UpdatedRows = ds.Tables[0].AsEnumerable()
                        .Where(row => InternalDocumentIds.Any(id => id == row["InternalDocumentId"].ToString()) && row["InternalDocumentStatus"].ToString().ToLower() == "updated").ToList();

                    if (UpdatedRows.Any())
                    {
                        UpdatedDocumentsIds = new List<string>();
                        UpdatedDocumentsIds = ds.Tables[0].AsEnumerable().Where(row => row["InternalDocumentStatus"].ToString().ToLower() == "updated")
                            .Select(row => row["InternalDocumentId"].ToString()).ToList();

                        for (int i = 0; i < UpdatedRows.Count; i++)
                        {
                            string SqlQuery = @"
                                        UPDATE DOCUMENTS SET [DocumentType] = @DocumentType ,[DocumentTypeVersion] = @DocumentTypeVersion, [DateTimeIssued] = @DateTimeIssued 
                                      , [TaxpayerActivityCode] = @TaxpayerActivityCode, [TotalSalesAmount]  = @TotalSalesAmount , [TotalDiscountAmount]  = @TotalDiscountAmount
                                      , [TotalItemsDiscountAmount]  = @TotalItemsDiscountAmount ,[ExtraDiscountAmount]  = @ExtraDiscountAmount
                                      , [NetAmount]  = @NetAmount , [TotalAmount]  = @TotalAmount
                                      , [IssuerId]  = @IssuerId , [IssuerName]  = @IssuerName , [IssuerType]   = @IssuerType ,[ModifiedOn] = GETDATE()
                                      , [IssuerBranchId]  = @IssuerBranchId , [IssuerCountry]  = @IssuerCountry , [IssuerGovernorate]  = @IssuerGovernorate
                                      , [IssuerRegionCity]  = @IssuerRegionCity , [IssuerStreet]  = @IssuerStreet , [IssuerBuildingNumber]  = @IssuerBuildingNumber
                                      , [ReceiverId] = @ReceiverId, [ReceiverGovernorate] = @ReceiverGovernorate, [ReceiverRegionCity] = @ReceiverRegionCity
                                      , [ReceiverStreet] = @ReceiverStreet, [ReceiverBuildingNumber] = @ReceiverBuildingNumber
                                      , [ReceiverName]  = @ReceiverName , [ReceiverType]  = @ReceiverType , [ReceiverCountry]  = @ReceiverCountry
                                      , [GrossWeight]  = @GrossWeight, [NetWeight]  = @NetWeight, [Status] = @Status  
                                        WHERE Id = @InternalDocumentId AND (STATUS = 'Failed' OR Status = 'Invalid')";

                            SqlCommand SqlCommand = new SqlCommand(SqlQuery, con);
                            SqlCommand.Parameters.AddWithValue("@DocumentType", UpdatedRows[i]["DocumentType"]);
                            SqlCommand.Parameters.AddWithValue("@DocumentTypeVersion", UpdatedRows[i]["DocumentTypeVersion"]);
                            SqlCommand.Parameters.AddWithValue("@TaxpayerActivityCode", UpdatedRows[i]["TaxpayerActivityCode"]);
                            SqlCommand.Parameters.AddWithValue("@TotalSalesAmount", Convert.ToDecimal(UpdatedRows[i]["TotalSalesAmount"]));
                            SqlCommand.Parameters.AddWithValue("@TotalDiscountAmount", Convert.ToDecimal(UpdatedRows[i]["TotalDiscountAmount"]));
                            SqlCommand.Parameters.AddWithValue("@TotalItemsDiscountAmount", Convert.ToDecimal(UpdatedRows[i]["TotalItemsDiscountAmount"]));
                            SqlCommand.Parameters.AddWithValue("@ExtraDiscountAmount", Convert.ToDecimal(UpdatedRows[i]["ExtraDiscountAmount"]));
                            SqlCommand.Parameters.AddWithValue("@NetAmount", Convert.ToDecimal(UpdatedRows[i]["NetAmount"]));
                            SqlCommand.Parameters.AddWithValue("@TotalAmount", Convert.ToDecimal(UpdatedRows[i]["TotalAmount"]));
                            SqlCommand.Parameters.AddWithValue("@IssuerId", UpdatedRows[i]["IssuerId"].ToString());
                            SqlCommand.Parameters.AddWithValue("@IssuerName", UpdatedRows[i]["IssuerName"]);
                            SqlCommand.Parameters.AddWithValue("@IssuerType", UpdatedRows[i]["IssuerType"]);
                            SqlCommand.Parameters.AddWithValue("@IssuerBranchId", UpdatedRows[i]["IssuerBranchId"]);
                            SqlCommand.Parameters.AddWithValue("@IssuerCountry", UpdatedRows[i]["IssuerCountry"]);
                            SqlCommand.Parameters.AddWithValue("@IssuerGovernorate", UpdatedRows[i]["IssuerGovernorate"]);
                            SqlCommand.Parameters.AddWithValue("@IssuerRegionCity", UpdatedRows[i]["IssuerRegionCity"]);
                            SqlCommand.Parameters.AddWithValue("@IssuerStreet", UpdatedRows[i]["IssuerStreet"]);
                            SqlCommand.Parameters.AddWithValue("@IssuerBuildingNumber", UpdatedRows[i]["IssuerBuildingNumber"]);
                            SqlCommand.Parameters.AddWithValue("@ReceiverId", UpdatedRows[i]["ReceiverId"].ToString());
                            SqlCommand.Parameters.AddWithValue("@ReceiverGovernorate", UpdatedRows[i]["ReceiverGovernorate"]);
                            SqlCommand.Parameters.AddWithValue("@ReceiverStreet", UpdatedRows[i]["ReceiverStreet"]);
                            SqlCommand.Parameters.AddWithValue("@ReceiverRegionCity", UpdatedRows[i]["ReceiverRegionCity"]);
                            SqlCommand.Parameters.AddWithValue("@ReceiverBuildingNumber", UpdatedRows[i]["ReceiverBuildingNumber"]);

                            SqlCommand.Parameters.AddWithValue("@ReceiverName", UpdatedRows[i]["ReceiverName"]);
                            SqlCommand.Parameters.AddWithValue("@ReceiverType", UpdatedRows[i]["ReceiverType"]);
                            SqlCommand.Parameters.AddWithValue("@ReceiverCountry", UpdatedRows[i]["ReceiverCountry"]);
                            SqlCommand.Parameters.AddWithValue("@GrossWeight", Convert.ToDecimal(UpdatedRows[i]["GrossWeight"]));
                            SqlCommand.Parameters.AddWithValue("@NetWeight", Convert.ToDecimal(UpdatedRows[i]["NetWeight"]));
                            SqlCommand.Parameters.AddWithValue("@Status", "New");
                            SqlCommand.Parameters.AddWithValue("@DateTimeReceived", UpdatedRows[i]["DateTimeReceived"]);
                            SqlCommand.Parameters.AddWithValue("@DateTimeIssued", UpdatedRows[i]["DateTimeIssued"]);
                            SqlCommand.Parameters.AddWithValue("@InternalDocumentId", UpdatedRows[i]["InternalDocumentId"].ToString());
                            SqlCommand.ExecuteNonQuery();
                        }
                        IsUpdated = true;
                        return UpdatedRows.Count;
                    }
                    else
                    {
                        NonExistingDocumentIds = new List<string>();
                        NonExistingDocumentIds = ds.Tables[0].AsEnumerable().Where(row => row["InternalDocumentStatus"].ToString().ToLower() == "updated")
                            .Select(row => row["InternalDocumentId"].ToString()).ToList();
                    }
                }
                
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int UpdateInvoiceLines()
        {
            List<DataRow> Lines = new List<DataRow>();
            Query = string.Format("Select " +
                                  "[InternalInvoiceLineId], [ItemType], [ItemCode], [UnitType], [InternalCode], [Quantity], [AmountEGP], [AmountSold], " +
                                  "[CurrencySold], [CurrencyExchangeRate], [SalesTotal], [DiscountRate], [DiscountAmount], [ItemsDiscount], [TotalTaxableFees], [ValueDifference]," +
                                  "[NetTotal], [Total], [Description], [InternalDocumentId] FROM [{0}]", "Invoice Lines$");
            
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);

            oda.Fill(ds);
            var UpdatedRows = ds.Tables[0].AsEnumerable().Where(row => UpdatedDocumentsIds.Any(id => id == row["InternalDocumentId"].ToString())).ToList();
            foreach (var currentRow in UpdatedRows)
            {
                currentRow.BeginEdit();
                currentRow["AmountEGP"] = Math.Round(Convert.ToDouble(currentRow["AmountEGP"]), 5);
                currentRow["AmountSold"] = Math.Round(Convert.ToDouble(currentRow["AmountSold"]), 5);
                currentRow["CurrencyExchangeRate"] = Math.Round(Convert.ToDouble(currentRow["CurrencyExchangeRate"]), 5);
                currentRow["SalesTotal"] = Math.Round(Convert.ToDouble(currentRow["SalesTotal"]), 5);
                currentRow["NetTotal"] = Math.Round(Convert.ToDouble(currentRow["NetTotal"]), 5);
                currentRow["Total"] = Math.Round(Convert.ToDouble(currentRow["Total"]), 5);
                currentRow.EndEdit();
                Lines.Add(currentRow);
            }

            if (Lines.Any())
            {
                UpdatedInvoiceLinesIds = new List<string>();
                UpdatedInvoiceLinesIds = ds.Tables[0].AsEnumerable().Where(row => UpdatedDocumentsIds.Any(id => id == row["InternalDocumentId"].ToString()))
                    .Select(row => row["InternalInvoiceLineId"].ToString()).ToList();
                for (int i = 0; i < UpdatedRows.Count; i++)
                {
                    string SqlQuery = @"UPDATE INVOICELINES SET [ItemType] = @ItemType, [ItemCode] = @ItemCode
                                       ,[UnitType] = @UnitType, [InternalCode] = @InternalCode , [Quantity] = @Quantity , [AmountEGP] = @AmountEGP, [AmountSold] = @AmountSold
                                       ,[CurrencySold] = @CurrencySold, [CurrencyExchangeRate] = @CurrencyExchangeRate, [SalesTotal] = @SalesTotal
                                       ,[DiscountRate] = @DiscountRate, [DiscountAmount] = @DiscountAmount, [ItemsDiscount] = @ItemsDiscount
                                       ,[TotalTaxableFees] = @TotalTaxableFees, [ValueDifference] = @ValueDifference , [ModifiedOn] = GETDATE()
                                       ,[NetTotal] = @NetTotal, [Total] = @Total, [Description] = @Description WHERE Id = @InternalInvoiceLineId";
                    SqlCommand SqlCommand = new SqlCommand(SqlQuery, con);
                    SqlCommand.Parameters.AddWithValue("@ItemType", UpdatedRows[i]["ItemType"]);
                    SqlCommand.Parameters.AddWithValue("@ItemCode", UpdatedRows[i]["ItemCode"]);
                    SqlCommand.Parameters.AddWithValue("@UnitType", UpdatedRows[i]["UnitType"]);
                    SqlCommand.Parameters.AddWithValue("@InternalCode", UpdatedRows[i]["InternalCode"]);
                    SqlCommand.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(UpdatedRows[i]["Quantity"]));
                    SqlCommand.Parameters.AddWithValue("@AmountEGP", Convert.ToDecimal(UpdatedRows[i]["AmountEGP"]));
                    SqlCommand.Parameters.AddWithValue("@AmountSold", Convert.ToDecimal(UpdatedRows[i]["AmountSold"]));
                    SqlCommand.Parameters.AddWithValue("@DiscountRate", Convert.ToDecimal(UpdatedRows[i]["DiscountRate"]));
                    SqlCommand.Parameters.AddWithValue("@DiscountAmount", Convert.ToDecimal(UpdatedRows[i]["DiscountAmount"]));
                    SqlCommand.Parameters.AddWithValue("@ItemsDiscount", Convert.ToDecimal(UpdatedRows[i]["ItemsDiscount"]));
                    SqlCommand.Parameters.AddWithValue("@CurrencySold", UpdatedRows[i]["CurrencySold"]);
                    SqlCommand.Parameters.AddWithValue("@CurrencyExchangeRate", Convert.ToDecimal(UpdatedRows[i]["CurrencyExchangeRate"]));
                    SqlCommand.Parameters.AddWithValue("@SalesTotal", Convert.ToDecimal(UpdatedRows[i]["SalesTotal"]));
                    SqlCommand.Parameters.AddWithValue("@TotalTaxableFees", Convert.ToDecimal(UpdatedRows[i]["TotalTaxableFees"]));
                    SqlCommand.Parameters.AddWithValue("@ValueDifference", Convert.ToDecimal(UpdatedRows[i]["ValueDifference"]));
                    SqlCommand.Parameters.AddWithValue("@NetTotal", Convert.ToDecimal(UpdatedRows[i]["NetTotal"]));
                    SqlCommand.Parameters.AddWithValue("@Total", Convert.ToDecimal(UpdatedRows[i]["Total"]));
                    SqlCommand.Parameters.AddWithValue("@Description", UpdatedRows[i]["Description"]);
                    //SqlCommand.Parameters.AddWithValue("@DocumentId", UpdatedRows[i]["InternalDocumentId"]);
                    SqlCommand.Parameters.AddWithValue("@InternalInvoiceLineId", UpdatedRows[i]["InternalInvoiceLineId"]);
                    SqlCommand.ExecuteNonQuery();

                }
                return UpdatedRows.Count;
            }
            return 0;
        }
        private int UpdateTaxableItems()
        {
            Query = string.Format("Select [InternalId],[TaxType], [Rate], [Amount], [SubType], [InternalInvoiceLineId] FROM [{0}]", "Taxable Items$");
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);

            oda.Fill(ds);
            var UpdatedRows = ds.Tables[0].AsEnumerable().Where(row => UpdatedInvoiceLinesIds.Any(id => id == row["InternalInvoiceLineId"].ToString())).ToList();
            if (UpdatedRows.Any())
            {
                for (int i = 0; i < UpdatedRows.Count; i++)
                {
                    string SqlQuery = @"UPDATE TAXABLEITEMS SET [TaxType] = cast(@TaxType as nvarchar(max)), [Rate] = cast(@Rate as float), [Amount] = cast(@Amount as float), [SubType] = cast (@SubType as nvarchar(max)) 
                                       , [ModifiedOn] = GETDATE() WHERE InternalId = @InternalId";
                    SqlCommand SqlCommand = new SqlCommand(SqlQuery, con);
                    SqlCommand.Parameters.AddWithValue("@TaxType", UpdatedRows[i]["TaxType"]);
                    SqlCommand.Parameters.AddWithValue("@Rate", UpdatedRows[i]["Rate"]);
                    SqlCommand.Parameters.AddWithValue("@Amount", UpdatedRows[i]["Amount"]);
                    SqlCommand.Parameters.AddWithValue("@SubType", UpdatedRows[i]["SubType"]);
                    //SqlCommand.Parameters.AddWithValue("@InternalInvoiceLineId", UpdatedRows[i]["InternalInvoiceLineId"]);
                    SqlCommand.Parameters.AddWithValue("@InternalId", UpdatedRows[i]["InternalId"]);
                    SqlCommand.ExecuteNonQuery();
                }
                return UpdatedRows.Count;
            }
            return 0;
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/importfromexcel")]
        public IHttpActionResult ImportFromExcel(string fullPath)
        {
            try
            {
                int UpdatedInvoiceLinesCount = 0;
                int UpdatedTaxableItemsCount = 0;
                int UpdatedDocumentsCount = 0;
                ExcelConn(fullPath);
                connection();
                Econ.Open();
                con.Open();
                int InsertedDocumentsCount = InsertDocuments();
                int InsertedInvoiceLinesCount = InsertInvoiceLine();
                int InsertedTaxableItemsCount = InsertTaxableItems();
                if (HasUpdatedRecords)
                {
                    UpdatedDocumentsCount = UpdateDocument();
                    if (UpdatedDocumentsCount > 0)
                    {
                        UpdatedInvoiceLinesCount = UpdateInvoiceLines();
                        UpdatedTaxableItemsCount = UpdateTaxableItems();
                    }
                }
                Econ.Close();
                con.Close();
                return Ok(new ImportedSheetDTO()
                {
                    InsertedDocumentsCount = InsertedDocumentsCount,
                    InsertedInvoiceLinesCount = InsertedInvoiceLinesCount,
                    InsertedTaxableItemsCount = InsertedTaxableItemsCount,
                    UpdatedDocumentsCount = UpdatedDocumentsCount,
                    UpdatedInvoiceLinesCount = UpdatedInvoiceLinesCount,
                    UpdatedTaxableItemsCount = UpdatedTaxableItemsCount,
                    NonExistingDocumentIds = NonExistingDocumentIds,
                    IsInserted = IsInserted,
                    //IsUpdated = IsUpdated,
                    UpdatesStatus = UpdatedDocumentsCount
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                Econ.Close();
                con.Close();
            }
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("api/document/updatedocumentsStatusfromETAToEIMC")]
        public IHttpActionResult UpdateDocumentsStatusFromETAToEIMC()
        {
            try
            {
                var regEX = new Regex(@"(?<!^)(?=[A-Z])", RegexOptions.IgnorePatternWhitespace);
                foreach (ConnectionStringSettings Item in ConfigurationManager.ConnectionStrings)
                {
                    if (Item.Name.StartsWith("EInvoice_") && Item.Name != "EInvoice_ProductOwner")
                    {
                        _documentService.GetTheConnectionString(Item.ConnectionString);
                        _userSession.SetBusinessGroup(regEX.Replace(Item.Name, " "));
                        //_userSession.SetBusinessGroup("Subsea7");
                        var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                        _documentService.GetRecentDocuments_ETA(_userSession.submissionurl, auth.access_token, 1500);
                    }
                }
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/document/getReceivedDocuments")]
        public IHttpActionResult GetReceivedDocuments()
        {
            try
            {
                var regEX = new Regex(@"(?<!^)(?=[A-Z])", RegexOptions.IgnorePatternWhitespace);
                foreach (ConnectionStringSettings Item in ConfigurationManager.ConnectionStrings)
                {
                    if (Item.Name.StartsWith("EInvoice_") && Item.Name != "EInvoice_ProductOwner")
                    {
                        _documentService.GetTheConnectionString(Item.ConnectionString);
                        _userSession.SetBusinessGroup(regEX.Replace(Item.Name, " "));
                        var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                        _documentService.GetReceivedDocuments(_userSession.submissionurl, auth.access_token, 1500);
                    }
                }
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/document/RetrieveDocumentInvalidityReasons")]
        public IHttpActionResult RetrieveDocumentInvalidityReasons()
        {
            try
            {
                var regEX = new Regex(@"(?<!^)(?=[A-Z])", RegexOptions.IgnorePatternWhitespace);
                foreach (ConnectionStringSettings Item in ConfigurationManager.ConnectionStrings)
                {
                    if (Item.Name.StartsWith("EInvoice_") && Item.Name != "EInvoice_ProductOwner")
                    {
                        _documentService.GetTheConnectionString(Item.ConnectionString);
                        //_userSession.SetBusinessGroup("Subsea7");
                        _userSession.SetBusinessGroup(regEX.Replace(Item.Name, " "));
                        var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                        _documentService.GetInvalidDocumentsReasone(_userSession.submissionurl, auth.access_token);
                    }
                }
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/document/SpecifyWhichActionsChain")]
        public IHttpActionResult SpecifyWhichActionsChain()
        {
            try
            {
                var regEX = new Regex(@"(?<!^)(?=[A-Z])", RegexOptions.IgnorePatternWhitespace);
                foreach (ConnectionStringSettings Item in ConfigurationManager.ConnectionStrings)
                {
                    if (Item.Name.StartsWith("EInvoice_") && Item.Name != "EInvoice_ProductOwner")
                    {
                        if (_userSession.IsDBSync(regEX.Replace(Item.Name, " ")))
                        {
                            string commandText = "EXEC [dbo].[SP_SyncDataFromViewsToTBLs]";
                            RunCommandAsynchronously(commandText, Item.ConnectionString, regEX.Replace(Item.Name, " "));
                        }
                        else
                        {
                            if (SubmitDocumentsPeriodically("BackGround_JOB", Item.ConnectionString, regEX.Replace(Item.Name, " ")))
                                continue;
                            else
                                break;
                        }
                    }
                }
                return Ok();
                //return Request.CreateResponse(HttpStatusCode.BadRequest, new { Success = true, RedirectUrl = newUrl });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/document/EIMCBackupPeriodically")]
        public IHttpActionResult EIMCBackupPeriodically()
        {
            try
            {
                foreach (ConnectionStringSettings Item in ConfigurationManager.ConnectionStrings)
                {
                    if (Item.Name.StartsWith("EInvoice_") && Item.Name != "EInvoice_ProductOwner")
                    {
                        using (SqlConnection myConnection = new SqlConnection(Item.ConnectionString))
                        {
                            string GroupName = Item.Name.Replace("EInvoice_", "");
                            myConnection.Open();
                            string SQLQuery  = @"BACKUP DATABASE " + GroupName + "_PreProd TO DISK = " + ConfigurationManager.AppSettings["Backup_Path"] + GroupName + "PreProd" + DateTime.Now.ToString("yyyyMMdd") + ".bak'";
                            string SQLQuery2 = @"BACKUP DATABASE " + GroupName + " TO DISK = " + ConfigurationManager.AppSettings["Backup_Path"] + GroupName + DateTime.Now.ToString("yyyyMMdd") + ".bak'";
                            SqlCommand command = new SqlCommand(SQLQuery+SQLQuery2, myConnection);
                            command.CommandTimeout = 600;
                            var output = command.ExecuteReader();
                            output.Close();
                            myConnection.Close();
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return InternalServerError();
            }
        }
        private void RunCommandAsynchronously(string commandText, string connectionString, string Name)
        {
            // Given command text and connection string, asynchronously execute
            // the specified command against the connection. For this example,
            // the code displays an indicator as it is working, verifying the
            // asynchronous behavior.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    int count = 0;
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();

                    IAsyncResult result = command.BeginExecuteNonQuery();
                    while (!result.IsCompleted)
                    {
                        Console.WriteLine("Waiting ({0})", count++);
                        System.Threading.Thread.Sleep(100);
                    }
                    Console.WriteLine("Command complete. Affected {0} rows.", command.EndExecuteNonQuery(result));
                    connection.Close();
                    SubmitDocumentsPeriodically("BackGround_JOB", connectionString, Name);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error ({0}): {1}", ex.Number, ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
            }
        }
        
        [LicenseAuthorization]
        private bool SubmitDocumentsPeriodically(string submittedBy, string CS, string Name)
        {
            Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
            try
            {
                _documentService.GetTheConnectionString(CS);
                _errorService.GetTheConnectionString(CS);
                _userSession.SetBusinessGroup(Name);
                DocumentSubmissionDTO Temp = new DocumentSubmissionDTO() { acceptedDocuments = new List<DocumentAcceptedDTO>(), rejectedDocuments = new List<DocumentRejectedDTO>() };
                var auth = _auth.token(_userSession.url, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                if(!string.IsNullOrEmpty(auth.access_token))
                {
                    if (objAppsettings != null)
                    {
                        objAppsettings.Settings["IsExternal"].Value = "1";
                        objAppsettings.Settings["Current_BusinessGroup"].Value = Name.Replace("E Invoice_ ", "");
                        objConfig.Save();
                    }
                    var _docs = _documentService.GetAllDocumentsToSubmit().ToList();
                    if (_docs.Count() > 0)
                    {
                        _docs.ForEach(i => i.documentTypeVersion = ConfigurationManager.AppSettings["TypeVersion"].ToLower() == "1.0" ? "1.0" : "0.9");
                        int totalPages = Convert.ToInt32(Math.Ceiling(_docs.Count() / 100.0));
                        SubmitInput paramaters;
                        for (int i = 0; i < totalPages; i++)
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                client.DefaultRequestHeaders.Clear();
                                client.Timeout = TimeSpan.FromMinutes(120);
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                var url = _userSession.submitServiceUrl + "api/InvoiceHasher/SubmitDocument";
                                client.BaseAddress = new Uri(url);
                                var _internalDocs = _documentService.GetAllDocumentsToSubmit();
                                if (_internalDocs.Count() < 100)
                                {
                                    paramaters = new SubmitInput()
                                    {
                                        documents = _internalDocs.ToList(),
                                        token = auth.access_token,
                                        pin = _userSession.pin,
                                        SRN = _userSession.SRN,
                                        url = _userSession.submissionurl,
                                        docuemntTypeVersion = ConfigurationManager.AppSettings["TypeVersion"].ToLower()
                                    };
                                }
                                else
                                {
                                    paramaters = new SubmitInput()
                                    {
                                        documents = _internalDocs.Take(100).ToList(),
                                        token = auth.access_token,
                                        pin = _userSession.pin,
                                        SRN = _userSession.SRN,
                                        url = _userSession.submissionurl,
                                        docuemntTypeVersion = ConfigurationManager.AppSettings["TypeVersion"].ToLower()
                                    };
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
                                        //Temp.submissionId = response.submissionId;
                                        if (response.acceptedDocuments != null)
                                            Temp.acceptedDocuments.AddRange(response.acceptedDocuments);
                                        if (response?.rejectedDocuments != null)
                                        {
                                            Temp.rejectedDocuments.AddRange(response.rejectedDocuments);
                                            _errorService.InsertBulk(response.rejectedDocuments);
                                        }
                                        _errorService.InsertBulk(response.rejectedDocuments);
                                        _documentService.UpdateDocuments(response, submittedBy);
                                    }
                                }
                            }
                            if (objAppsettings != null)
                            {
                                objAppsettings.Settings["IsExternal"].Value = "0";
                            }
                        }
                        _documentService.NotifyBusinessGroupWithSubmissionStatus(new EmailContentDTO()
                        { SentCount = Temp.acceptedDocuments.Count(), FailedCount = Temp.rejectedDocuments.Count(), BusinessGroup = Name.Replace("E Invoice_ ", "") });
                    }
                    else
                    {
                        _documentService.NotifyBusinessGroupWithSubmissionStatus(new EmailContentDTO()
                        { SentCount = Temp.acceptedDocuments.Count(), FailedCount = Temp.rejectedDocuments.Count(), BusinessGroup = Name.Replace("E Invoice_ ", "")});
                    }
                }
                if (objAppsettings != null)
                {
                    objAppsettings.Settings["IsExternal"].Value = "0";
                }
                return false;
            }
            catch
            {
                if (objAppsettings != null)
                {
                    objAppsettings.Settings["IsExternal"].Value = "0";
                }
                return false;
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/SyncCustomerDocumentsByCurrentloggedinOrg")]
        public IHttpActionResult SyncCustomerDocumentsByCurrentloggedinOrg()
        {
            try
            {
                string commandText = "EXEC [dbo].[SP_SyncDataFromViewsToTBLs]";
                if (RunCommandwithoutSubmitAsynchronously(commandText, this.OnActionExecuting()))
                    return Ok();
                else
                    return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private bool RunCommandwithoutSubmitAsynchronously(string commandText, string connectionString)
        {
            // Given command text and connection string, asynchronously execute
            // the specified command against the connection. For this example,
            // the code displays an indicator as it is working, verifying the
            // asynchronous behavior.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    int count = 0;
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();

                    IAsyncResult result = command.BeginExecuteNonQuery();
                    while (!result.IsCompleted)
                    {
                        Console.WriteLine("Waiting ({0})", count++);
                        System.Threading.Thread.Sleep(100);
                    }
                    Console.WriteLine("Command complete. Affected {0} rows.", command.EndExecuteNonQuery(result));
                    connection.Close();
                    return true;
                    //SubmitDocumentsPeriodically("BackGround_JOB", connectionString, Name);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error ({0}): {1}", ex.Number, ex.Message);
                    return false;
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                    return false;
                }
            }
        }


        [JwtAuthentication]
        [HttpGet]
        [Route("api/document/InvalidandFailed")]
        public IHttpActionResult InvalidandFailed(int pageNumber, int pageSize, DateTime fromDate, 
            DateTime toDate, string searchValue, string sortColumnName, string sortDirection, string status)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                var docs = _documentService.GetInvalidandFailedDocuments(pageNumber, pageSize, fromDate, toDate, searchValue, sortColumnName, sortDirection, status);
                return Ok(new DocumentResponse() { meta = new Meta() { page = docs.CurrentPage, pages = docs.TotalPages, perpage = docs.PageSize, total = docs.TotalCount }, data = docs });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        [JwtAuthentication]
        [HttpPost]
        [Route("api/document/ResyncInvalidandFailedDocuments")]
        public int ResyncInvalidandFailedDocuments(List<string> DocumentIds)
        {
            try
            {
                //int count = 0;
                DocumentCollection docColl = new DocumentCollection();
                docColl.AddRange(DocumentIds.Select(i => new DocumentDetail() { DocumentId = i }).ToList());

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Doc_Id_List";
                param.SqlDbType = SqlDbType.Structured;
                param.Value = docColl;
                param.Direction = ParameterDirection.Input;

                SqlParameter outparam = new SqlParameter();
                outparam.ParameterName = "@DocumentsAffetced";
                outparam.SqlDbType = SqlDbType.Int;
                outparam.Direction = ParameterDirection.Output;

                SqlConnection conn = null;

                using (conn = new SqlConnection(this.OnActionExecuting()))
                {
                    SqlCommand sqlCmd = new SqlCommand("[dbo].[SP_ReSyncDataFromViewsToTBLs]");
                    conn.Open();
                    sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add(param);
                    sqlCmd.Parameters.Add(outparam);
                    sqlCmd.CommandTimeout = 3600;
                    IAsyncResult result = sqlCmd.ExecuteNonQueryAsync();
                    while (!result.IsCompleted)
                    {
                        //Console.WriteLine("Waiting ({0})", count++);
                        System.Threading.Thread.Sleep(100);
                    }
                    int res = Convert.ToInt32(sqlCmd.Parameters["@DocumentsAffetced"].Value);
                    //Console.WriteLine("Command complete. Affected {0} rows.", sqlCmd.EndExecuteNonQuery(result));
                    conn.Close();
                    return res;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error ({0}): {1}", ex.Number, ex.Message);
                return -1;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return -1;
            }
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("api/document/ResyncAllInvalidandFailedDocuments")]
        public int ResyncAllInvalidandFailedDocuments()
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                var DocumentIds = _documentService.GetAllInvalidandFailedDocumentsIds();

                DocumentCollection docColl = new DocumentCollection();
                docColl.AddRange(DocumentIds.Select(i => new DocumentDetail() { DocumentId = i }).ToList());

                SqlParameter param = new SqlParameter();
                param.ParameterName = "Doc_Id_List";
                param.SqlDbType = SqlDbType.Structured;
                param.Value = docColl;
                param.Direction = ParameterDirection.Input;

                SqlParameter outparam = new SqlParameter();
                outparam.ParameterName = "DocumentsAffetced";
                outparam.SqlDbType = SqlDbType.Int;
                outparam.Direction = ParameterDirection.Output;

                SqlConnection conn = null;

                using (conn = new SqlConnection(this.OnActionExecuting()))
                {
                    SqlCommand sqlCmd = new SqlCommand("[dbo].[SP_ReSyncDataFromViewsToTBLs]");
                    conn.Open();
                    sqlCmd.Connection = conn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add(param);
                    sqlCmd.Parameters.Add(outparam);
                    sqlCmd.CommandTimeout = 3600;
                    IAsyncResult result = sqlCmd.ExecuteNonQueryAsync();
                    while (!result.IsCompleted)
                    {
                        //Console.WriteLine("Waiting ({0})", count++);
                        System.Threading.Thread.Sleep(100);
                    }
                    //Console.WriteLine("Command complete. Affected {0} rows.", sqlCmd.EndExecuteNonQuery(result));
                    conn.Close();
                    dynamic res = result;
                    return res.Result;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error ({0}): {1}", ex.Number, ex.Message);
                return -1;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return - 1;
            }

        }

        [JwtAuthentication]
        [HttpPost]
        [Route("api/document/RecallInvalidandFailedDocuments")]
        public IHttpActionResult RecallInvalidandFailedDocuments(List<string> DocumentIds)
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                _documentService.UpdateBulkDocumentsByIds(DocumentIds);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("api/document/RecallAllInvalidandFailedDocuments")]
        public IHttpActionResult RecallAllInvalidandFailedDocuments()
        {
            try
            {
                _documentService.GetTheConnectionString(this.OnActionExecuting());
                _documentService.UpdateBulkDocumentsByStatus();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}