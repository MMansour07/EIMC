﻿using eInvoicing.DTO.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class NewDocumentVM : BaseDTO
    {
        public string Id { get; set; }
        public string InternalId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentTypeVersion { get; set; }
        public string TaxPayerNameEn { get; set; }
        public string RegistrationNumber { get; set; }
        public string ProformaInvoiceNumber { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal ExtraDiscountAmount { get; set; }
        public decimal TotalItemsDiscountAmount { get; set; }
        public string TaxpayerActivityCode { get; set; }
        public string PurchaseOrderReference { get; set; }
        public string PurchaseOrderDescription { get; set; }
        public string SalesOrderReference { get; set; }
        public string SalesOrderDescription { get; set; }
        public string IssuerId { get; set; }
        public string IssuerName { get; set; }
        public string IssuerType { get; set; }
        public string IssuerBranchId { get; set; }
        public string IssuerCountry { get; set; }
        public string IssuerGovernorate { get; set; }
        public string IssuerRegionCity { get; set; }
        public string IssuerStreet { get; set; }
        public string IssuerBuildingNumber { get; set; }
        public string IssuerPostalCode { get; set; }
        public string IssuerFloor { get; set; }
        public string IssuerRoom { get; set; }
        public string IssuerLandmark { get; set; }
        public string IssuerAdditionalInformation { get; set; }
        public string RGN { get; set; }
        public string NID { get; set; }
        public string PID { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverType { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverGovernorate { get; set; }
        public string ReceiverRegionCity { get; set; }
        public string ReceiverStreet { get; set; }
        public string ReceiverBuildingNumber { get; set; }
        public string ReceiverPostalCode { get; set; }
        public string ReceiverFloor { get; set; }
        public string ReceiverRoom { get; set; }
        public string ReceiverLandmark { get; set; }
        public string ReceiverAdditionalInformation { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string BankAccountNo { get; set; }
        public string BankAccountIBAN { get; set; }
        public string SwiftCode { get; set; }
        public string PaymentTerms { get; set; }
        public string Approach { get; set; }
        public string Packaging { get; set; }
        public DateTime? DateValidity { get; set; }
        public string ExportPort { get; set; }
        public string CountryOfOrigin { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }
        public string DeliveryTerms { get; set; }
        public DateTime DateTimeIssued { get; set; }
        public DateTime DateTimeReceived { get; set; }
        public string uuid { get; set; }
        public string submissionId { get; set; }
        public string longId { get; set; }
        public string Status { get; set; }
        public string SubmittedBy { get; set; }
        public string InvalidReason { get; set; }
        public string ParentId { get; set; }
        public bool isInternallyCreated { get; set; }
        public List<NewInvoiceLineVM> InvoiceLines { get; set; }
    }

    public class doc
    {
        public NewDocumentVM Document { get; set; }
        public List<NewInvoiceLineVM> InvoiceLines { get; set; }
    }

}