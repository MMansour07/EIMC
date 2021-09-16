using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class AttributeDTO: BaseDTO
    {
        public string createdByUserId { get; set; }
        public DateTime creationDateTimeUtc { get; set; }
        public int codeAttributeID { get; set; }
        public int codeID { get; set; }
        public string codeAttributeKey { get; set; }
        public CodeAttributeValueDTO codeAttributeValue { get; set; }
    }
}