using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class GetCodeDetailsbyItemCodeResultDTO: BaseDTO
    {
        public int codeID { get; set; }
        public string codeLookupValue { get; set; }
        public string codeNamePrimaryLang { get; set; }
        public string codeNameSecondaryLang { get; set; }
        public string codeDescriptionPrimaryLang { get; set; }
        public string codeDescriptionSecondaryLang { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime activeTo { get; set; }
        public int parentCodeID { get; set; }
        public string parentCodeLookupValue { get; set; }
        public int codeTypeID { get; set; }
        public int codeTypeLevelID { get; set; }
        public string codeTypeLevelNamePrimaryLang { get; set; }
        public string codeTypeLevelNameSecondaryLang { get; set; }
        public string parentCodeNamePrimaryLang { get; set; }
        public string parentCodeNameSecondaryLang { get; set; }
        public string parentLevelName { get; set; }
        public string codeTypeNamePrimaryLang { get; set; }
        public string codeTypeNameSecondaryLang { get; set; }
        public string linkedCode { get; set; }
        public bool active { get; set; }
        public CodeCategorization codeCategorization { get; set; }
        public OwnerTaxpayer ownerTaxpayer { get; set; }
        public RequesterTaxpayer requesterTaxpayer { get; set; }
        public List<AttributeDTO> attributes { get; set; }
    }
}