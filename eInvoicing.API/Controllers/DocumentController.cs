using eInvoicing.API.Filters;
using eInvoicing.API.Models;
using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using ProductLicense;
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
using WebApi.Jwt.Filters;

namespace eInvoicing.API.Controllers
{

    [JwtAuthentication]
    public class DocumentController : ApiController
    {
        private readonly IDocumentService _documentService;
        private readonly IAuthService _auth;
        private readonly IUserSession _userSession;
        OleDbConnection Econ;
        SqlConnection con;
        string constr, Query, sqlconn;
        bool IsInserted, IsUpdated = false;
        List<string> UpdatedDocumentsIds;
        List<string> UpdatedInvoiceLinesIds;

        public DocumentController(IDocumentService documentService, IAuthService auth, IUserSession userSession)
        {
            _documentService = documentService;
            _auth = auth;
            _userSession = userSession;
        }
        [HttpGet]
        [Route("api/document/pending")]
        public IHttpActionResult Pending(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, string searchValue, string sortColumnName, string sortDirection, string status)
        {
            try
            {
                var docs = _documentService.GetPendingDocuments(pageNumber, pageSize, fromDate, toDate, searchValue, sortColumnName, sortDirection, status);
                return Ok(new DocumentResponse() { meta = new Meta() { page = docs.CurrentPage, pages = docs.TotalPages, perpage = docs.PageSize, total = docs.TotalCount }, data = docs });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/document/pendingCount")]
        public IHttpActionResult PendingCount()
        {
            try
            {
                return Ok(_documentService.GetPendingCount());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/document/MonthlyDocuments")]
        public IHttpActionResult GetMonthlyDocuments(DateTime _date)
        {
            try
            {
                return Ok(_documentService.GetMonthlyDocuments(_date));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/document/submitted")]
        public IHttpActionResult Submitted(int pageNumber, int pageSize, string searchValue, string sortColumnName, string sortDirection, string status)
        {
            try
            {
                var docs = _documentService.GetSubmittedDocuments(pageNumber, pageSize, searchValue, sortColumnName, sortDirection, status);
                return Ok(new DocumentResponse() { meta = new Meta() { page = docs.CurrentPage, pages = docs.TotalPages, perpage = docs.PageSize, total = docs.TotalCount }, data = docs });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/document/submittedCount")]
        public IHttpActionResult submittedCount()
        {
            try
            {
                return Ok(_documentService.GetSubmittedCount());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/document/GetByDocumentId")]
        public IHttpActionResult GetByDocumentId(string id)
        {
            try
            {
                var docs = _documentService.GetDocumentById(id);
                return Ok(docs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/document/GetDocumentByuuid")]
        public IHttpActionResult GetDocumentByuuid(string uuid)
        {
            try
            {
                var docs = _documentService.GetDocumentByuuid(uuid);
                return Ok(docs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/document/recent")]
        public IHttpActionResult GetRecentDocuments_ETA(int pageNo, int pageSize)
        {
            try
            {
                var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                var result = _documentService.GetRecentDocuments_ETA2(_userSession.submissionurl, auth.access_token, pageNo, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        [HttpGet]
        [Route("api/document/raw")]
        public IHttpActionResult Raw(string uuid)
        {
            try
            {
                var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                var result = _documentService.GetDocument_ETA(_userSession.submissionurl, auth.access_token, uuid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        public IHttpActionResult GetDocumentPrintOut(string uuid)
        {
            var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
            var result = _documentService.GetDocumentPrintOut(_userSession.submissionurl, auth.access_token, uuid);
            return Ok(result);
        }
        public IHttpActionResult CancelDocument(string uuid, string reason)
        {
            var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
            var result = _documentService.CancelDocument(_userSession.submissionurl, auth.access_token, uuid, reason);
            return Ok(result);
        }
        public IHttpActionResult RejectDocument(string uuid, string reason)
        {
            var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
            var result = _documentService.RejectDocument(_userSession.submissionurl, auth.access_token, uuid, reason);
            return Ok(result);
        }
        public IHttpActionResult DeclineDocumentCancellation(string uuid)
        {
            var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
            var result = _documentService.DeclineDocumentCancellation(_userSession.submissionurl, auth.access_token, uuid);
            return Ok(result);
        }
        public IHttpActionResult DeclineDocumentRejection(string uuid)
        {
            var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
            var result = _documentService.DeclineDocumentRejection(_userSession.submissionurl, auth.access_token, uuid);
            return Ok(result);
        }

        [Route("api/document/updatedocuments")]
        public IHttpActionResult UpdateDocuments(DocumentSubmissionDTO obj, string submittedBy)
        {
            try
            {
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

            constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", FilePath);
            Econ = new OleDbConnection(constr);

        }
        private void connection()
        {
            sqlconn = "Data Source=.;Initial Catalog=Lazurdi_DB;User Id=sa;Password=123";
            con = new SqlConnection(sqlconn);

        }

        private int InsertDocuments()
        {
            Query = string.Format("Select [DocumentType],[DocumentTypeVersion], [TaxpayerActivityCode], [DateTimeIssued], [InternalDocumentId], [TotalSalesAmount], [TotalDiscountAmount], [TotalItemsDiscountAmount], " +
                                    "[ExtraDiscountAmount], [NetAmount], [TotalAmount], [IssuerId], [IssuerName], [IssuerType], [IssuerBranchId], [IssuerCountry], [IssuerGovernate]," +
                                    " [IssuerRegionCity], [IssuerStreet], [IssuerBuildingNumber], [ReceiverName], [ReceiverType], [ReceiverCountry], [GrossWeight], [NetWeight], [InternalDocumentStatus] FROM [{0}]", "Document$");
            string SQLQuery = "SELECT [Id] FROM DOCUMENTS ";
            List<string> InternalDocumentIds = new List<string>();

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

            var NewRows = ds.Tables[0].AsEnumerable().Where(row => !InternalDocumentIds.Any(id => id == row["InternalDocumentId"].ToString()) && 
            (row["InternalDocumentStatus"].ToString().ToLower() == "completed" || row["InternalDocumentStatus"].ToString().ToLower() == "updated"));
            if (NewRows.Any())
            {
                Exceldt = NewRows.CopyToDataTable();
                IsInserted = true;
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
            objbulk.ColumnMappings.Add("IssuerGovernate", "IssuerGovernate");
            objbulk.ColumnMappings.Add("IssuerRegionCity", "IssuerRegionCity");
            objbulk.ColumnMappings.Add("IssuerStreet", "IssuerStreet");
            objbulk.ColumnMappings.Add("IssuerBuildingNumber", "IssuerBuildingNumber");
            objbulk.ColumnMappings.Add("ReceiverName", "ReceiverName");
            objbulk.ColumnMappings.Add("ReceiverType", "ReceiverType");
            objbulk.ColumnMappings.Add("ReceiverCountry", "ReceiverCountry");
            objbulk.ColumnMappings.Add("GrossWeight", "GrossWeight");
            objbulk.ColumnMappings.Add("NetWeight", "NetWeight");
            objbulk.ColumnMappings.Add("InternalDocumentStatus", "Status");

            //inserting Datatable Records to DataBase  
            //con.Open();
            if (Exceldt != null)
            {
                objbulk.WriteToServer(Exceldt);
                return Exceldt.Rows.Count;
            }
            return 0;
        }

        private int InsertInvoiceLine()
        {
            Query = string.Format("Select [InternalInvoiceLineId],[ItemType], [ItemCode], [UnitType], [InternalCode], [Quantity], [AmountEGP], [AmountSold], " +
                "[CurrencySold], [CurrencyExchangeRate], [SalesTotal], [DiscountRate], [DiscountAmount], [ItemsDiscount], [TotalTaxableFees], [ValueDifference]," +
                " [NetTotal], [Total], [Description], [DocumentId] FROM [{0}]", "Lines$");
            string SQLQuery = "SELECT [InternalInvoiceLineId] FROM INVOICELINES";
            List<string> InternalInvoiceLinesIds = new List<string>();

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

            var NewRows = ds.Tables[0].AsEnumerable().Where(row => !InternalInvoiceLinesIds.Any(id => id == row["InternalInvoiceLineId"].ToString()));
            if (NewRows.Any())
            {
                Exceldt = NewRows.CopyToDataTable();
                IsInserted = true;
            }
            //creating object of SqlBulkCopy  
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            //assigning Destination table name  
            objbulk.DestinationTableName = "INVOICELINES";
            //Mapping Table column  
            objbulk.ColumnMappings.Add("InternalInvoiceLineId", "InternalInvoiceLineId");
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
            objbulk.ColumnMappings.Add("DocumentId", "DocumentId");
            if (Exceldt != null)
            {
                objbulk.WriteToServer(Exceldt);
                return Exceldt.Rows.Count;
            }
            return 0;

        }

        private int InsertTaxableItems()
        {
            Query = string.Format("Select [InternalId],[TaxType], [Rate], [Amount], [SubType], [InternalInvoiceLineId] FROM [{0}]", "Taxable items$");
            string SQLQuery = "SELECT  CONCAT(InternalId,InvoiceLineId) AS 'UniqueTaxableItemKey'  FROM TAXABLEITEMS";
            List<string> UniqueTaxableItemKeys = new List<string>();
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

            var NewRows = ds.Tables[0].AsEnumerable().Where(row => !UniqueTaxableItemKeys.Any(key => key == row["InternalId"].ToString() + row["InternalInvoiceLineId"].ToString()));
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
            Query = string.Format("Select [DocumentType],[DocumentTypeVersion], [TaxpayerActivityCode], [DateTimeIssued], [InternalDocumentId], [TotalSalesAmount], [TotalDiscountAmount], [TotalItemsDiscountAmount], " +
                "[ExtraDiscountAmount], [NetAmount], [TotalAmount], [IssuerId], [IssuerName], [IssuerType], [IssuerBranchId], [IssuerCountry], [IssuerGovernate]," +
                " [IssuerRegionCity], [IssuerStreet], [IssuerBuildingNumber], [ReceiverName], [ReceiverType], [ReceiverCountry], [GrossWeight], [NetWeight], [InternalDocumentStatus] FROM [{0}]", "Document$");

            string SQLQuery = "SELECT [Id] FROM DOCUMENTS where Lower(Status) = 'updated'";
            List<string> InternalDocumentIds = new List<string>();

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
            var UpdatedRows = ds.Tables[0].AsEnumerable().Where(row => InternalDocumentIds.Any(id => id == row["InternalDocumentId"].ToString())  && row["InternalDocumentStatus"].ToString().ToLower() == "updated").ToList();
            if (UpdatedRows.Any())
            {
                UpdatedDocumentsIds = new List<string>();
                UpdatedDocumentsIds = ds.Tables[0].AsEnumerable().Where(row => row["InternalDocumentStatus"].ToString().ToLower() == "updated").Select(row => row["InternalDocumentId"].ToString()).ToList();
                for (int i = 0; i < UpdatedRows.Count; i++)
                {
                    string SqlQuery = @"UPDATE DOCUMENTS SET [DocumentType] = @DocumentType ,[DocumentTypeVersion] = @DocumentTypeVersion 
                                      , [TaxpayerActivityCode] = @TaxpayerActivityCode, [TotalSalesAmount]  = @TotalSalesAmount , [TotalDiscountAmount]  = @TotalDiscountAmount
                                      , [TotalItemsDiscountAmount]  = @TotalItemsDiscountAmount ,[ExtraDiscountAmount]  = @ExtraDiscountAmount
                                      , [NetAmount]  = @NetAmount , [TotalAmount]  = @TotalAmount
                                      , [IssuerId]  = @IssuerId , [IssuerName]  = @IssuerName , [IssuerType]   = @IssuerType ,[ModifiedOn] = GETDATE()
                                      , [IssuerBranchId]  = @IssuerBranchId , [IssuerCountry]  = @IssuerCountry , [IssuerGovernate]  = @IssuerGovernate
                                      ,[IssuerRegionCity]  = @IssuerRegionCity , [IssuerStreet]  = @IssuerStreet , [IssuerBuildingNumber]  = @IssuerBuildingNumber
                                      , [ReceiverName]  = @ReceiverName , [ReceiverType]  = @ReceiverType , [ReceiverCountry]  = @ReceiverCountry
                                      , [GrossWeight]  = @GrossWeight, [NetWeight]  = @NetWeight , [Status]  = @InternalDocumentStatus WHERE Id = @InternalDocumentId";
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
                    SqlCommand.Parameters.AddWithValue("@IssuerId", UpdatedRows[i]["IssuerId"]);
                    SqlCommand.Parameters.AddWithValue("@IssuerName", UpdatedRows[i]["IssuerName"]);
                    SqlCommand.Parameters.AddWithValue("@IssuerType", UpdatedRows[i]["IssuerType"]);
                    SqlCommand.Parameters.AddWithValue("@IssuerBranchId", UpdatedRows[i]["IssuerBranchId"]);
                    SqlCommand.Parameters.AddWithValue("@IssuerCountry", UpdatedRows[i]["IssuerCountry"]);
                    SqlCommand.Parameters.AddWithValue("@IssuerGovernate", UpdatedRows[i]["IssuerGovernate"]);
                    SqlCommand.Parameters.AddWithValue("@IssuerRegionCity", UpdatedRows[i]["IssuerRegionCity"]);
                    SqlCommand.Parameters.AddWithValue("@IssuerStreet", UpdatedRows[i]["IssuerStreet"]);
                    SqlCommand.Parameters.AddWithValue("@IssuerBuildingNumber", UpdatedRows[i]["IssuerBuildingNumber"]);
                    SqlCommand.Parameters.AddWithValue("@ReceiverName", UpdatedRows[i]["ReceiverName"]);
                    SqlCommand.Parameters.AddWithValue("@ReceiverType", UpdatedRows[i]["ReceiverType"]);
                    SqlCommand.Parameters.AddWithValue("@ReceiverCountry", UpdatedRows[i]["ReceiverCountry"]);
                    SqlCommand.Parameters.AddWithValue("@GrossWeight", Convert.ToDecimal(UpdatedRows[i]["GrossWeight"]));
                    SqlCommand.Parameters.AddWithValue("@NetWeight", Convert.ToDecimal(UpdatedRows[i]["NetWeight"]));
                    SqlCommand.Parameters.AddWithValue("@InternalDocumentStatus", UpdatedRows[i]["InternalDocumentStatus"]);
                    SqlCommand.Parameters.AddWithValue("@InternalDocumentId", UpdatedRows[i]["InternalDocumentId"]);
                    SqlCommand.ExecuteNonQuery();
                }

                //Exceldt = UpdatedRows.CopyToDataTable();

                IsUpdated = true;
                return UpdatedRows.Count;
            }

            return 0;
        }
        private int UpdateInvoiceLines()
        {
            Query = string.Format("Select [InternalInvoiceLineId],[ItemType], [ItemCode], [UnitType], [InternalCode], [Quantity], [AmountEGP], [AmountSold], " +
                "[CurrencySold], [CurrencyExchangeRate], [SalesTotal], [DiscountRate], [DiscountAmount], [ItemsDiscount], [TotalTaxableFees], [ValueDifference]," +
                " [NetTotal], [Total], [Description], [DocumentId] FROM [{0}]", "Lines$");
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);

            oda.Fill(ds);
            var UpdatedRows = ds.Tables[0].AsEnumerable().Where(row => UpdatedDocumentsIds.Any(id => id == row["DocumentId"].ToString())).ToList();
            if (UpdatedRows.Any())
            {
                UpdatedInvoiceLinesIds = new List<string>();
                UpdatedInvoiceLinesIds = ds.Tables[0].AsEnumerable().Where(row => UpdatedDocumentsIds.Any(id => id == row["DocumentId"].ToString())).Select(row => row["InternalInvoiceLineId"].ToString()).ToList();
                for (int i = 0; i < UpdatedRows.Count; i++)
                {
                    string SqlQuery = @"UPDATE INVOICELINES SET [ItemType] = @ItemType, [ItemCode] = @ItemCode
                                       , [UnitType] = @UnitType, [InternalCode] = @InternalCode , [Quantity] = @Quantity , [AmountEGP] = @AmountEGP, [AmountSold] = @AmountSold
                                       ,[CurrencySold] = @CurrencySold, [CurrencyExchangeRate] = @CurrencyExchangeRate, [SalesTotal] = @SalesTotal
                                       , [DiscountRate] = @DiscountRate, [DiscountAmount] = @DiscountAmount, [ItemsDiscount] = @ItemsDiscount
                                       , [TotalTaxableFees] = @TotalTaxableFees, [ValueDifference] = @ValueDifference , [ModifiedOn] = GETDATE()
                                       ,[NetTotal] = @NetTotal, [Total] = @Total, [Description] = @Description, [DocumentId] = @DocumentId WHERE [InternalInvoiceLineId] = @InternalInvoiceLineId";
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
                    SqlCommand.Parameters.AddWithValue("@DocumentId", UpdatedRows[i]["DocumentId"]);
                    SqlCommand.Parameters.AddWithValue("@InternalInvoiceLineId", UpdatedRows[i]["InternalInvoiceLineId"]);
                    SqlCommand.ExecuteNonQuery();

                }
                return UpdatedRows.Count;
            }
            return 0;
        }
        private int UpdateTaxableItems()
        {
            Query = string.Format("Select [InternalId],[TaxType], [Rate], [Amount], [SubType], [InternalInvoiceLineId] FROM [{0}]", "Taxable items$");
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);

            oda.Fill(ds);
            var UpdatedRows = ds.Tables[0].AsEnumerable().Where(row => UpdatedInvoiceLinesIds.Any(id => id == row["InternalInvoiceLineId"].ToString())).ToList();
            if (UpdatedRows.Any())
            {
                for (int i = 0; i < UpdatedRows.Count; i++)
                {
                    string SqlQuery = @"UPDATE TAXABLEITEMS SET [TaxType] = @TaxType, [Rate] = @Rate, [Amount] = @Amount, [SubType] = @SubType, [InvoiceLineId] = @InternalInvoiceLineId
                                        ,[ModifiedOn] = GETDATE() WHERE InternalId = @InternalId";
                    SqlCommand SqlCommand = new SqlCommand(SqlQuery, con);
                    SqlCommand.Parameters.AddWithValue("@TaxType", UpdatedRows[i]["TaxType"]);
                    SqlCommand.Parameters.AddWithValue("@Rate", UpdatedRows[i]["Rate"]);
                    SqlCommand.Parameters.AddWithValue("@Amount", UpdatedRows[i]["Amount"]);
                    SqlCommand.Parameters.AddWithValue("@SubType", UpdatedRows[i]["SubType"]);
                    SqlCommand.Parameters.AddWithValue("@InternalInvoiceLineId", UpdatedRows[i]["InternalInvoiceLineId"]);
                    SqlCommand.Parameters.AddWithValue("@InternalId", UpdatedRows[i]["InternalId"]);
                    SqlCommand.ExecuteNonQuery();
                }
                return UpdatedRows.Count;
            }
            return 0;
        }
        [HttpGet]
        [Route("api/document/importfromexcel")]
        public IHttpActionResult ImportFromExcel(string fullPath)
        {
            try
            {
                int UpdatedInvoiceLinesCount = 0;
                int UpdatedTaxableItemsCount = 0;
                ExcelConn(fullPath);
                connection();
                Econ.Open();
                con.Open();
                int InsertedDocumentsCount = InsertDocuments();
                int InsertedInvoiceLinesCount = InsertInvoiceLine();
                int InsertedTaxableItemsCount = InsertTaxableItems();
                int UpdatedDocumentsCount = UpdateDocument();
                if (UpdatedDocumentsCount > 0)
                {
                    UpdatedInvoiceLinesCount = UpdateInvoiceLines();
                    UpdatedTaxableItemsCount = UpdateTaxableItems();
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
                    IsInserted = IsInserted,
                    IsUpdated = IsUpdated
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

    }
}