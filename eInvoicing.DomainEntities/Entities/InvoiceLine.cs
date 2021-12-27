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
        [DataType("decimal(18, 5)")]
        public decimal Quantity { get; set; }
        public string InternalCode { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal SalesTotal { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal Total { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal ValueDifference { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal TotalTaxableFees { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal NetTotal { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal ItemsDiscount { get; set; }
        public string Description { get; set; }
        public string CurrencySold { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal AmountEGP { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal AmountSold { get; set; }

        [DataType("decimal(18, 5)")]
        public decimal CurrencyExchangeRate { get; set; }
        public decimal DiscountRate { get; set; }
        [DataType("decimal(18, 5)")]
        public decimal DiscountAmount { get; set; }
        public string DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }
        public virtual ICollection<TaxableItem> TaxableItems { get; set; }

    }
}
