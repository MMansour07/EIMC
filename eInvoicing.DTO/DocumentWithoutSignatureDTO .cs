using eInvoicing.DTO.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class DocumentWithoutSignatureDTO : BaseDTO
    {
        public ISSUERSDTO issuer { get; set; }
        public RECEIVERSDTO receiver { get; set; }
        public PAYMENTSDTO payment { get; set; }
        public DELIVERIESDTO delivery { get; set; }
        public DateTime dateTimeIssued { get; set; }
        public string taxpayerActivityCode { get; set; }
        public string internalID { get; set; }
        public string purchaseOrderReference { get; set; }
        public string purchaseOrderDescription { get; set; }
        public string salesOrderReference { get; set; }
        public string salesOrderDescription { get; set; }
        public string documentType { get; set; }
        public string documentTypeVersion { get; set; }
        public string proformaInvoiceNumber { get; set; }
        public decimal totalDiscountAmount { get; set; }
        public decimal totalSalesAmount { get; set; }
        public decimal netAmount { get; set; }
        public decimal totalAmount { get; set; }
        public decimal extraDiscountAmount { get; set; }
        public decimal totalItemsDiscountAmount { get; set; }
        //public List<string> references { get; set; }
        public List<INVOICELINESDTO> invoiceLines { get; set; }
        public List<TAXTOTALSDTO> taxTotals { get; set; }
    }
}
