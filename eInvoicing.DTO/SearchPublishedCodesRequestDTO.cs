using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class SearchPublishedCodesRequestDTO: BaseDTO
    {
        public string codeType { get; set; }
        public string codeLookupValue { get; set; }
        public string parentCodeLookupValue { get; set; }
        public string parentLevelName { get; set; }
        public string TaxpayerRIN { get; set; }
        public int codeID { get; set; }
        public int ParentCodeID { get; set; }
        public string codeName { get; set; }
        public string codeDescription { get; set; }
        public Boolean onlyActive { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime activeTo { get; set; }
        public string ps { get; set; }
        public string pn { get; set; }
        public int codeTypeLevelNumber { get; set; }
        public string orderDirections { get; set; }

    }
}
