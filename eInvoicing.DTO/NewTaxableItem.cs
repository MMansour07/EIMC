using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class NewTaxableItem : BaseDTO
    {
        public string TaxType { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string SubType { get; set; }
    }
}

