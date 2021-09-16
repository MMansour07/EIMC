using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class ISSUERSDTO: BaseDTO
    {
        public ISSUERADDESSESDTO address { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }

    }
}
