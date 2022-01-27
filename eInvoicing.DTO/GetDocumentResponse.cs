using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class GetDocumentResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public DocumentVM document { get; set; }
        public string transformationStatus { get; set; }
        public ValidationResultsDTO validationResults { get; set; }
        public int maxPercision { get; set; }
        public List<InvoiceLineItemCodesDTO> invoiceLineItemCodes { get; set; }
        //public List<InvoiceLineItemCodesDTO> invoiceLines { get; set; }
        public string uuid { get; set; }
        public string submissionUUID { get; set; }
        public string longId { get; set; }
        public string internalId { get; set; }
        public string typeName { get; set; }
        public string typeVersionName { get; set; }
        public string issuerId { get; set; }
        public string issuerName { get; set; }
        public string receiverId { get; set; }
        public string receiverName { get; set; }
        public string dateTimeIssued { get; set; }
        public string dateTimeReceived { get; set; }
        public string dateTimeRecevied { get; set; }
        public decimal totalSales { get; set; }
        public decimal totalDiscount { get; set; }
        public decimal netAmount { get; set; }
        public decimal total { get; set; }
        public decimal totalAmount { get; set; }
        public decimal totalItemsDiscountAmount { get; set; }
        public string documentStatusReason { get; set; }
        public DateTime? cancelRequestDate { get; set; }
        public DateTime? rejectRequestDate { get; set; }
        public DateTime? cancelRequestDelayedDate { get; set; }
        public DateTime? rejectRequestDelayedDate { get; set; }
        public DateTime? declineCancelRequestDate { get; set; }
        public DateTime? declineRejectRequestDate { get; set; }
        public DateTime? canbeCancelledUntil { get; set; }
        public DateTime? canbeRejectedUntil { get; set; }
        public decimal totalTax
        {
            get
            {
                return decimal.Round(total - netAmount, 2, MidpointRounding.AwayFromZero);
            }
        }
        public string status { get; set; }
        public string statusClass { get; set; }

        public ISSUERSDTO issuer { get; set; }
        public RECEIVERSDTO receiver { get; set; }
        public PAYMENTSDTO payment { get; set; }
        public DELIVERIESDTO delivery { get; set; }
        //public DateTime dateTimeIssued { get; set; }
        
        //public DateTime? dateTimeReceived { get; set; }
        //public DateTime? dateTimeRecevied { get; set; }
        public string taxpayerActivityCode { get; set; }
        public string internalID { get; set; }
        public string recordID { get; set; }
        //public string status { get; set; }
        public string purchaseOrderReference { get; set; }
        public string purchaseOrderDescription { get; set; }
        public string salesOrderReference { get; set; }
        public string salesOrderDescription { get; set; }
        public string documentType { get; set; }
        public string documentTypeVersion { get; set; }
        public string proformaInvoiceNumber { get; set; }
        public string totalDiscountAmount { get; set; }
        public string totalSalesAmount { get; set; }
        //public string netAmount { get; set; }
        public string taxAmount { get; set; }
        //public string totalAmount { get; set; }
        public string extraDiscountAmount { get; set; }
        //public string totalItemsDiscountAmount { get; set; }
        public string IssuerFullAddress { get; set; }
        public string ReceiverFullAddress { get; set; }
        //public string uuid { get; set; }
        //public string submissionUUID { get; set; }
        //public string longId { get; set; }
        public bool isInternallyCreated { get; set; }
        public bool? IsCancelRequested { get; set; }
        public bool? IsDeclineRequested { get; set; }
        //public string documentStatusReason { get; set; }
        //public DateTime? cancelRequestDate { get; set; }
        //public DateTime? rejectRequestDate { get; set; }
        //public DateTime? declineCancelRequestDate { get; set; }
        //public DateTime? declineRejectRequestDate { get; set; }
        //public DateTime? canbeCancelledUntil { get; set; }
        //public DateTime? canbeRejectedUntil { get; set; }
        //public DateTime? cancelRequestDelayedDate { get; set; }
        //public DateTime? rejectRequestDelayedDate { get; set; }
        public List<INVOICELINESVM> invoiceLines { get; set; }
        public List<CustomErrorDTO> errors { get; set; }
        public List<TAXTOTALSDTO> taxTotals { get; set; }
        //public ValidationResultsDTO validationResults { get; set; }
        public string parentId { get; set; }
    }
}
