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
    public class TaxableItem: BaseEntity
    {
        [Key]
        public new int Id { get; set; }
        public string  InternalId { get; set; }
        public string TaxType { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string SubType { get; set; }
        public string InvoiceLineId { get; set; }
        [ForeignKey("InvoiceLineId")]
        public virtual InvoiceLine InvoiceLine { get; set; }
    }
}
