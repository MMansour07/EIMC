using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class InvalidDocumentsReasonsDTO : BaseDTO
    {
        public string DocumentId { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ExtraDiscountAmount { get; set; }
        public decimal TotalItemsDiscountAmount { get; set; }
        public DateTime? DateTimeIssued { get; set; }
        public DateTime? DateTimeReceived { get; set; }
        public string ValidationSteps { get; set; }
        public string Errors { get; set; }
        public List<string> Temp { get; set; }
        public string InnerErrors { get; set; }
    }
}
