using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class DISCOUNTSVM: BaseDTO
    {
        public string rate { get; set; }
        public string amount { get; set; }
    }
}
