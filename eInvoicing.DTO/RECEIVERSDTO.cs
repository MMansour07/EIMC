using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class RECEIVERSDTO: BaseDTO
    {
        public RECEIVERADDRESSESDTO address { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }
}
