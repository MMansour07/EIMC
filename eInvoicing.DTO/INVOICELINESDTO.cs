using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class INVOICELINESDTO : BaseDTO
    {
        public string itemType { get; set; }
        public string itemCode { get; set; }
        public string unitType { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal quantity { get; set; }
        public string internalCode { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal salesTotal { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal total { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal valueDifference { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal totalTaxableFees { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal netTotal { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal itemsDiscount { get; set; }
        public string description { get; set; }
        public UNITVALUESDTO unitValue { get; set; }
        public DISCOUNTSDTO discount { get; set; }
        public List<TAXABLEITEMSDTO> taxableItems { get; set; }
    }
      
}
