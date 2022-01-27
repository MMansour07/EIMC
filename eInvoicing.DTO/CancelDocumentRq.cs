using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class CancelDocumentRq
    {
        public string status { get; set; }
        public string reason { get; set; }
    }
}
