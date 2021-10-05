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
        public int maxPercision {  get; set; }
        public List<InvoiceLineItemCodesDTO> invoiceLineItemCodes { get; set; }
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
        public decimal totalSales { get; set; }
        public decimal totalDiscount { get; set; }
        public decimal netAmount { get; set; }
        public decimal total { get; set; }
        public string status { get; set; }
        public string statusClass { get; set; }
    }
}
