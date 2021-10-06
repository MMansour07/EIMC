using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using eInvoicing.Repository.Contract.Base;
using eInvoicing.DomainEntities.Entities;
using Ninject;
using eInvoicing.Repository.Contract;
using System.Reflection;
using eInvoicing.Repository.Implementation;
using System.Data.SqlClient;
using System.Globalization;

namespace eInvoicing.Service.AppService.Implementation
{
    public class SyncService: ISyncService
    {
        private readonly IDocumentRepository _DocRepo;
        private readonly IInvoiceLineRepository _LineRepo;
        private readonly ITaxableItemRepository _ItemRepo;
        protected readonly IUnitOfWork _unitOfWork;
        public SyncService(IDocumentRepository DocRepo, IInvoiceLineRepository LineRepo, ITaxableItemRepository ItemRepo, IUnitOfWork unitOfWork)
        {
            _DocRepo = DocRepo;
            _LineRepo = LineRepo;
            _ItemRepo = ItemRepo;
            _unitOfWork = unitOfWork;
        }

        public List<Document> GetDocumentsFromVW_Documents(string _connectionstring, string IssuedDate)
        {
            List<Document> DocumentsList = new List<Document>();
            try
            {
                using (SqlConnection myConnection = new SqlConnection(_connectionstring))
                {

                    string oString = "Select * from VW_Documents where DateTimeIssued = convert(date,Getdate()) and InternalDocumentId not in (select id from Documents)";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    oCmd.Parameters.AddWithValue("@IssuedDate", IssuedDate);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        if (oReader.HasRows)
                        {
                            while (oReader.Read())
                            {
                                Document obj = new Document();
                                obj.Id = oReader["InternalDocumentId"].ToString();
                                obj.Approach = oReader["Approach"].ToString();
                                obj.BankAccountIBAN = oReader["BankAccountIBAN"].ToString();
                                obj.BankAccountNo = oReader["BankAccountNo"].ToString();
                                obj.BankAddress = oReader["BankAddress"].ToString();
                                obj.BankName = oReader["BankName"].ToString();
                                obj.CountryOfOrigin = oReader["CountryOfOrigin"].ToString();
                                obj.DateTimeIssued =  DateTime.ParseExact(oReader["DateTimeIssued"].ToString(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                                if (!String.IsNullOrEmpty(oReader["DateValidity"].ToString()))
                                {
                                    obj.DateValidity = DateTime.ParseExact(oReader["DateValidity"].ToString(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                                }
                                obj.DeliveryTerms = oReader["DeliveryTerms"].ToString();
                                obj.DocumentType = oReader["DocumentType"].ToString();
                                obj.DocumentTypeVersion = oReader["DocumentTypeVersion"].ToString();
                                obj.ExportPort = oReader["ExportPort"].ToString();
                                obj.ExtraDiscountAmount = !String.IsNullOrEmpty(oReader["ExtraDiscountAmount"].ToString()) ? Decimal.Parse(oReader["ExtraDiscountAmount"].ToString()) : 0;
                                obj.GrossWeight = !String.IsNullOrEmpty(oReader["GrossWeight"].ToString()) ? Decimal.Parse(oReader["GrossWeight"].ToString()) : 0;
                                obj.IssuerAdditionalInformation = oReader["IssuerAdditionalInformation"].ToString();
                                obj.IssuerBranchId = oReader["IssuerBranchId"].ToString();
                                obj.IssuerBuildingNumber = oReader["IssuerBuildingNumber"].ToString();
                                obj.IssuerCountry = oReader["IssuerCountry"].ToString();
                                obj.IssuerFloor = oReader["IssuerFloor"].ToString();
                                obj.IssuerGovernorate = oReader["IssuerGovernate"].ToString();
                                obj.IssuerId = oReader["IssuerId"].ToString();
                                obj.IssuerLandmark = oReader["IssuerLandmark"].ToString();
                                obj.IssuerName = oReader["IssuerName"].ToString();
                                obj.IssuerPostalCode = oReader["IssuerPostalCode"].ToString();
                                obj.IssuerRegionCity = oReader["IssuerRegionCity"].ToString();
                                obj.IssuerRoom = oReader["IssuerRoom"].ToString();
                                obj.IssuerStreet = oReader["IssuerStreet"].ToString();
                                obj.IssuerType = oReader["IssuerType"].ToString();
                                obj.ReceiverAdditionalInformation = oReader["ReceiverAdditionalInformation"].ToString();
                                obj.ReceiverBuildingNumber = oReader["ReceiverBuildingNumber"].ToString();
                                obj.ReceiverCountry = oReader["ReceiverCountry"].ToString();
                                obj.ReceiverFloor = oReader["ReceiverFloor"].ToString();
                                obj.ReceiverGovernorate = oReader["ReceiverGovernate"].ToString();
                                obj.ReceiverId = oReader["ReceiverId"].ToString();
                                obj.ReceiverLandmark = oReader["ReceiverLandmark"].ToString();
                                obj.ReceiverName = oReader["ReceiverName"].ToString();
                                obj.ReceiverPostalCode = oReader["ReceiverPostalCode"].ToString();
                                obj.ReceiverRegionCity = oReader["ReceiverRegionCity"].ToString();
                                obj.ReceiverRoom = oReader["ReceiverRoom"].ToString();
                                obj.ReceiverStreet = oReader["ReceiverStreet"].ToString();
                                obj.ReceiverType = oReader["ReceiverType"].ToString();
                                obj.NetAmount = !String.IsNullOrEmpty(oReader["NetAmount"].ToString()) ? Decimal.Parse(oReader["NetAmount"].ToString()) : 0;
                                obj.NetWeight = !String.IsNullOrEmpty(oReader["NetWeight"].ToString()) ? Decimal.Parse(oReader["NetWeight"].ToString()) : 0;
                                obj.Packaging = oReader["Packaging"].ToString();
                                obj.PaymentTerms = oReader["PaymentTerms"].ToString();
                                obj.ProformaInvoiceNumber = oReader["ProformaInvoiceNumber"].ToString();
                                obj.PurchaseOrderDescription = oReader["PurchaseOrderDescription"].ToString();
                                obj.PurchaseOrderReference = oReader["PurchaseOrderReference"].ToString();
                                obj.SalesOrderDescription = oReader["SalesOrderDescription"].ToString();
                                obj.SalesOrderReference = oReader["SalesOrderReference"].ToString();
                                obj.SwiftCode = oReader["SwiftCode"].ToString();
                                obj.TaxpayerActivityCode = oReader["TaxpayerActivityCode"].ToString();
                                obj.TotalAmount = !String.IsNullOrEmpty(oReader["TotalAmount"].ToString()) ? Decimal.Parse(oReader["TotalAmount"].ToString()) : 0;
                                obj.TotalDiscountAmount = !String.IsNullOrEmpty(oReader["TotalDiscountAmount"].ToString()) ? Decimal.Parse(oReader["TotalDiscountAmount"].ToString()) : 0;
                                obj.TotalItemsDiscountAmount = !String.IsNullOrEmpty(oReader["TotalItemsDiscountAmount"].ToString()) ? Decimal.Parse(oReader["TotalItemsDiscountAmount"].ToString()) : 0;
                                obj.TotalSalesAmount = !String.IsNullOrEmpty(oReader["TotalSalesAmount"].ToString()) ? Decimal.Parse(oReader["TotalSalesAmount"].ToString()) : 0;
                                obj.Status = "New";
                                DocumentsList.Add(obj);
                            }
                        }
                        else
                        {
                            myConnection.Close();
                        }
                    }
                }
                return DocumentsList;
            }

            catch (Exception ex)
            {
                return new List<Document>();
            }
        }

        public List<InvoiceLine> GetInvoiceLinesFromVW_InvoiceLines(string _connectionstring, string IssuedDate)
        {
            List<InvoiceLine> InvoiceLinesList = new List<InvoiceLine>();
            try
            {
                using (SqlConnection myConnection = new SqlConnection(_connectionstring))
                {

                    string oString = "select * from VW_InvoiceLines I inner join VW_Documents D on D.InternalDocumentId = I.DocumentId where D.DateTimeIssued = convert(date,@IssuedDate) and I.InternalInvoiceLineId not in (select Id from InvoiceLines)";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    oCmd.Parameters.AddWithValue("@IssuedDate", IssuedDate);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        if (oReader.HasRows)
                        {
                            while (oReader.Read())
                            {
                                InvoiceLine obj = new InvoiceLine();
                                obj.Id = oReader["InternalInvoiceLineId"].ToString();
                                obj.DiscountAmount = !String.IsNullOrEmpty(oReader["Amount"].ToString()) ? Decimal.Parse(oReader["Amount"].ToString()) : 0;
                                oReader["Amount"].ToString();
                                obj.AmountEGP = !String.IsNullOrEmpty(oReader["AmountEGP"].ToString()) ? Decimal.Parse(oReader["AmountEGP"].ToString()) : 0;
                                obj.AmountSold = !String.IsNullOrEmpty(oReader["AmountSold"].ToString()) ? Decimal.Parse(oReader["AmountSold"].ToString()) : 0;
                                obj.CurrencyExchangeRate = !String.IsNullOrEmpty(oReader["CurrencyExchangeRate"].ToString()) ? Decimal.Parse(oReader["CurrencyExchangeRate"].ToString()) : 0;
                                obj.CurrencySold = oReader["CurrencySold"].ToString();
                                obj.Description = oReader["Description"].ToString();
                                obj.InternalCode = oReader["InternalCode"].ToString();
                                obj.ItemCode = oReader["ItemCode"].ToString();
                                obj.ItemsDiscount = !String.IsNullOrEmpty(oReader["ItemsDiscount"].ToString()) ? Decimal.Parse(oReader["ItemsDiscount"].ToString()) : 0;
                                obj.ItemType = oReader["ItemType"].ToString();
                                obj.NetTotal = !String.IsNullOrEmpty(oReader["NetTotal"].ToString()) ? Decimal.Parse(oReader["NetTotal"].ToString()) : 0;
                                obj.Quantity = !String.IsNullOrEmpty(oReader["Quantity"].ToString()) ? Decimal.Parse(oReader["Quantity"].ToString()) : 0;
                                obj.DiscountRate = !String.IsNullOrEmpty(oReader["Rate"].ToString()) ? Decimal.Parse(oReader["Rate"].ToString()) : 0;
                                obj.SalesTotal = !String.IsNullOrEmpty(oReader["SalesTotal"].ToString()) ? Decimal.Parse(oReader["SalesTotal"].ToString()) : 0;
                                obj.Total = !String.IsNullOrEmpty(oReader["Total"].ToString()) ? Decimal.Parse(oReader["Total"].ToString()) : 0;
                                obj.TotalTaxableFees = !String.IsNullOrEmpty(oReader["TotalTaxableFees"].ToString()) ? Decimal.Parse(oReader["TotalTaxableFees"].ToString()) : 0;
                                obj.UnitType = oReader["UnitType"].ToString();
                                obj.ValueDifference = !String.IsNullOrEmpty(oReader["ValueDifference"].ToString()) ? Decimal.Parse(oReader["ValueDifference"].ToString()) : 0;
                                obj.DocumentId = oReader["DocumentId"].ToString();
                                InvoiceLinesList.Add(obj);
                            }
                        }
                        else
                        {
                            myConnection.Close();
                        }
                    }
                }
                return InvoiceLinesList;
            }

            catch (Exception ex)
            {
                return new List<InvoiceLine>();
            }
        }

        public List<TaxableItem> GetTaxableItemsFromVW_TaxableItems(string _connectionstring, string IssuedDate)
        {
            List<TaxableItem> TaxableItemsList = new List<TaxableItem>();
            try
            {
                using (SqlConnection myConnection = new SqlConnection(_connectionstring))
                {

                    string oString = "select T.Id, T.TaxType, T.SubType, T.TaxAmount, T.TaxRate, T.InvoiceLineId from VW_InvoiceLines I inner join VW_Documents D on D.InternalDocumentId = I.DocumentId inner join VW_TaxableItems T on I.InternalInvoiceLineId = T.InvoiceLineId where D.DateTimeIssued = convert(date, @IssuedDate) and T.Id not in (select Id from TaxableItems)";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    oCmd.Parameters.AddWithValue("@IssuedDate", IssuedDate);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        if (oReader.HasRows)
                        {
                            while (oReader.Read())
                            {
                                TaxableItem obj = new TaxableItem();
                                obj.Id = Convert.ToInt32(oReader["Id"]);
                                obj.Amount = !String.IsNullOrEmpty(oReader["TaxAmount"].ToString()) ? Decimal.Parse(oReader["TaxAmount"].ToString()) : 0;
                                obj.Rate = !String.IsNullOrEmpty(oReader["TaxRate"].ToString()) ? Decimal.Parse(oReader["TaxRate"].ToString()) : 0;
                                obj.TaxType = oReader["TaxType"].ToString();
                                obj.SubType = oReader["SubType"].ToString();
                                obj.InvoiceLineId = oReader["InvoiceLineId"].ToString();
                                TaxableItemsList.Add(obj);
                            }
                        }
                        else
                        {
                            myConnection.Close();
                        }
                    }
                }
                return TaxableItemsList;
            }

            catch (Exception ex)
            {
                return new List<TaxableItem>();
            }
        }

        public List<Document> InsertBulkIntoDocumentsTbl(string _connectionstring, string IssuedDate)
        {
            try
            {
                var documents = this.GetDocumentsFromVW_Documents(_connectionstring, IssuedDate);
                _DocRepo.AddRange(documents);
                return new List<Document>();
            }
            catch (Exception ex)
            {
                return new List<Document>();
            }
        }

        public List<InvoiceLine> InsertBulkIntoInvoiceLinesTbl(string _connectionstring, string IssuedDate)
        {
            try 
            {
                var invoiceLines = this.GetInvoiceLinesFromVW_InvoiceLines(_connectionstring, IssuedDate);
                 _LineRepo.AddRange(invoiceLines);
                //_unitOfWork.Save();
                return new List<InvoiceLine>();
            }
            
            catch
            {
                return new List<InvoiceLine>();
            }
        }

        public List<TaxableItem> InsertBulkIntoTaxableItemsTbl(string _connectionstring, string IssuedDate)
        {
            try
            {
                var taxableItems = this.GetTaxableItemsFromVW_TaxableItems(_connectionstring, IssuedDate);
                 _ItemRepo.AddRange(taxableItems);
                //_unitOfWork.Save();
                return new List<TaxableItem>();
            }
            catch
            {
                return new List<TaxableItem>();
            }
        }
        public void SyncFromViewToTbl()
        {
            string con = ConfigurationManager.ConnectionStrings["eInvoicing_CS"].ToString();
            try
            {
                 this.InsertBulkIntoDocumentsTbl(con, DateTime.Now.ToString());
                 this.InsertBulkIntoInvoiceLinesTbl(con, DateTime.Now.ToString());
                 this.InsertBulkIntoTaxableItemsTbl(con, DateTime.Now.ToString());
            }
            catch
            {
                throw new NotImplementedException();
            }

        }
    }
}
