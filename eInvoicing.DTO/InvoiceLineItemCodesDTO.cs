using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class InvoiceLineItemCodesDTO
    {
        public int codeTypeId { get; set; }
        public string codeTypeNamePrimaryLang { get; set; }
        public string codeTypeNameSecondaryLang { get; set; }
        public string itemCode { get; set; }
        public string codeNamePrimaryLang { get; set; }
        public string codeNameSecondaryLang { get; set; }
    }
}
