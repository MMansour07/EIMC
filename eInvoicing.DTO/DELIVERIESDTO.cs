using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class DELIVERIESDTO: BaseDTO
    {
        public string approach { get; set; }
        public string packaging { get; set; }
        public string dateValidity { get; set; }
        public string exportPort { get; set; }
        public string countryOfOrigin { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal grossWeight { get; set; }
        [RegularExpression(@"^\d+\.\d{0,5}$")]
        [Range(0, 9999999999999999.99999)]
        public decimal netWeight { get; set; }
        public string terms { get; set; }
    }
}
