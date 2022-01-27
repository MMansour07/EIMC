using eInvoicing.DomainEntities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DomainEntities.Entities
{
    public class ValidationStep: BaseEntity
    {
        public string Status { get; set; }
        public string StepName { get; set; }
        public string StepId { get; set; }
        public string DocumentId { get; set; }

        [DataType("decimal(18, 5)")]
        public decimal TotalDiscountAmount { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal TotalSalesAmount { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal NetAmount { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal TotalAmount { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal ExtraDiscountAmount { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal TotalItemsDiscountAmount { get; set; }
        public DateTime? DateTimeIssued { get; set; }
        public DateTime? DateTimeReceived { get; set; }
        public virtual ICollection<StepError> StepErrors { get; set; }
        
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }
    }
}
