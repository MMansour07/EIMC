using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class DISCOUNTSDTO: BaseDTO
    {
        public decimal rate { get; set; }
        public decimal amount { get; set; }
    }
}
