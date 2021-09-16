using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class UpdateCodeRequestDTO: BaseDTO
    {
        public string codeType { get; set; }
        public string itemCode { get; set; }
        public string codeDescriptionPrimaryLang { get; set; }
        public string codeDescriptionSecondaryLang { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime activeTo { get; set; }
        public string linkedCode { get; set; }
    }
}
