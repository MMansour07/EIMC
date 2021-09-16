using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class CodeAttributeValueDTO: BaseDTO
    {
        public string name { get; set; }
        public string arabic_name { get; set; }
    }
}
