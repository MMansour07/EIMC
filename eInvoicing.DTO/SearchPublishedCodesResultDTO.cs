using eInvoicing.DTO.Base;
using System;

namespace eInvoicing.DTO
{
    public class SearchPublishedCodesResultDTO : BaseDTO
    {
        public int codeID { get; set; }
        public string codeLookupValue { get; set; }
        public string codeNamePrimaryLang { get; set; }
        public string codeNameSecondaryLang { get; set; }
        public string codeDescriptionPrimaryLang { get; set; }
        public string codeDescriptionSecondaryLang { get; set; }
        public string activeFrom { get; set; }
        public string activeTo { get; set; }
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
        public string net_content { get; set; }
        public string net_content_arabic { get; set; }
        public string brand { get; set; }
        public bool active { get; set; }
        public string linkedCode { get; set; }
        public OwnerTaxpayer ownerTaxpayer { get; set; }
        public RequesterTaxpayer requesterTaxpayer { get; set; }
        public CodeCategorization CodeCategorization { get; set; }
    }
}
