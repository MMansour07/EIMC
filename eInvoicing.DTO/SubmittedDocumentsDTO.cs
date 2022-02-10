using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class SubmittedDocumentsDTO : BaseDTO
    {
        public DateTime issuedOn { get; set; }
        public DateTime? submittedOn { get; set; }
        public int totalCount { get; set; }
        public int totalFiltered { get; set; }
        public int validCount { get; set; }
        public int invalidCount { get; set; }
        public int submittedCount { get; set; }
        public int cancelledCount { get; set; }
        public int rejectedCount { get; set; }
        public int invoiceCount { get; set; }
        public int creditCount { get; set; }
        public int debitCount { get; set; }
        public decimal totalSalesAmount { get; set; }
        public decimal netAmount { get; set; }
        public decimal totalAmount { get; set; }
        public decimal totalDiscountAmount { get; set; }
        public decimal totalItemsDiscountAmount { get; set; }
        public string submittedBy { get; set; }
        public string reason { get; set; }
    }
}
