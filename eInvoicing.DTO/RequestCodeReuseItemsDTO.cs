using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class RequestCodeReuseItemsDTO: BaseDTO
    {
        public string codetype { get; set; }
        public string itemCode { get; set; }
        public string comment { get; set; }
    }
}