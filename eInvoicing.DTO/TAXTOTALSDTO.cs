using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class TAXTOTALSDTO: BaseDTO
    {
        public string taxType { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal amount { get; set; }
    }
}
