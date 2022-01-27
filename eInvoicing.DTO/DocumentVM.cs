using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;

namespace eInvoicing.DTO
{
    public class DocumentVM : BaseDTO
    {
        public ISSUERSDTO issuer { get; set; }
        public RECEIVERSDTO receiver { get; set; }
        public PAYMENTSDTO payment { get; set; }
        public DELIVERIESDTO delivery { get; set; }
        public DateTime dateTimeIssued { get; set; }
        public DateTime dateTimeReceived { get; set; }
        public string taxpayerActivityCode { get; set; }
        public string internalID { get; set; }
        public string recordID { get; set; }
        public string status { get; set; }
        public string statusClass { get; set; }
        public string purchaseOrderReference { get; set; }
        public string purchaseOrderDescription { get; set; }
        public string salesOrderReference { get; set; }
        public string salesOrderDescription { get; set; }
        public string documentType { get; set; }
        public string documentTypeVersion { get; set; }
        public string proformaInvoiceNumber { get; set; }
        public string totalDiscountAmount { get; set; }
        public string totalSalesAmount { get; set; }
        public string netAmount { get; set; }
        public string taxAmount { get; set; }
        public string totalAmount { get; set; }
        public string extraDiscountAmount { get; set; }
        public string totalItemsDiscountAmount { get; set; }
        public string IssuerFullAddress { get; set; }
        public string ReceiverFullAddress { get; set; }
        public string uuid { get; set; }
        public string submissionUUID { get; set; }
        public string longId { get; set; }
        public bool isInternallyCreated { get; set; }
        public bool? IsCancelRequested { get; set; }
        public bool? IsDeclineRequested { get; set; }
        public string DocumentStatusReason { get; set; }
        public DateTime? CancelRequestDate { get; set; }
        public DateTime? RejectRequestDate { get; set; }
        public DateTime? DeclineCancelRequestDate { get; set; }
        public DateTime? DeclineRejectRequestDate { get; set; }
        public DateTime? CanbeCancelledUntil { get; set; }
        public DateTime? CanbeRejectedUntil { get; set; }
        public DateTime? CancelRequestDelayedDate { get; set; }
        public DateTime? RejectRequestDelayedDate { get; set; }
        public List<INVOICELINESVM> invoiceLines { get; set; }
        public List<CustomErrorDTO> errors { get; set; }
        public List<TAXTOTALSDTO> taxTotals { get; set; }
        public string parentId { get; set; }
        
    }
}
