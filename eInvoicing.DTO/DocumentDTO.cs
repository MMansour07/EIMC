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
    public class DocumentDTO : BaseDTO
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
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal totalDiscountAmount { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal totalSalesAmount { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal netAmount { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal totalAmount { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal extraDiscountAmount { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal totalItemsDiscountAmount { get; set; }
        public List<INVOICELINESDTO> invoiceLines { get; set; }
        public List<TAXTOTALSDTO> taxTotals { get; set; }
        public List<SIGNATURESDTO> signatures { get; set; }
    }
}
