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
    public class InvoiceLine : BaseEntity
    {
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string UnitType { get; set; }
        public decimal Quantity { get; set; }
        public string InternalCode { get; set; }
        public decimal SalesTotal { get; set; }
        public decimal Total { get; set; }
        public decimal ValueDifference { get; set; }
        public decimal TotalTaxableFees { get; set; }
        public decimal NetTotal { get; set; }
        public decimal ItemsDiscount { get; set; }
        public string Description { get; set; }
        public string CurrencySold { get; set; }
        public decimal AmountEGP { get; set; }
        public decimal AmountSold { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }
        public virtual ICollection<TaxableItem> TaxableItems { get; set; }

    }
}
