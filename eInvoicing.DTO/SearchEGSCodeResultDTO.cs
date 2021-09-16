using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class SearchEGSCodeResultDTO : BaseDTO
    {
        public int codeUsageRequestID { get; set; }
        public string codeTypeName { get; set; }
        public int codeID { get; set; }
        public string itemCode { get; set; }
        public string codeName { get; set; }
        public string description { get; set; }
        public int parentCodeID { get; set; }
        public string parentItemCode { get; set; }
        public string parentLevelName { get; set; }
        public string levelName { get; set; }
        public DateTime requestCreationDateTimeUtc { get; set; }
        public DateTime codeCreationDateTimeUtc { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime activeTo { get; set; }
        public Boolean active { get; set; }
        public string status { get; set; }
        public OwnerTaxpayer ownerTaxpayer { get; set; }
        public RequesterTaxpayer requesterTaxpayer { get; set; }
        public CodeCategorization CodeCategorization { get; set; }
    }
}
