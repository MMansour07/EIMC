using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class OwnerTaxpayer: BaseDTO
    {
        public string rin { get; set; }
        public string name { get; set; }
        public string nameAr { get; set; }
    }
}
