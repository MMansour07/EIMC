using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class SIGNATURESDTO: BaseDTO
    {
        public string signatureType { get; set; }
        public string value { get; set; }
    }
}
