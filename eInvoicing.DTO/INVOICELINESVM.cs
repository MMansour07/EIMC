using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class INVOICELINESVM : BaseDTO
    {
        public string itemType { get; set; }
        public string internalId { get; set; }
        public string itemCode { get; set; }
        public string unitType { get; set; }
        public string internalCode { get; set; }
        public string quantity { get; set; }
        public string salesTotal { get; set; }
        public string total { get; set; }
        public string valueDifference { get; set; }
        public string totalTaxableFees { get; set; }
        public string totalTax { get; set; }
        public decimal taxAmount {
            get
            {
                return decimal.Round(Convert.ToDecimal(total) - Convert.ToDecimal(netTotal), 2, MidpointRounding.AwayFromZero);
            }
        }
        public string netTotal { get; set; }
        public string itemsDiscount { get; set; }
        public string description { get; set; }
        public UNITVALUESVM unitValue { get; set; }
        public DISCOUNTSVM discount { get; set; }
        public List<TAXABLEITEMSVM> taxableItems { get; set; }
    }
      
}
