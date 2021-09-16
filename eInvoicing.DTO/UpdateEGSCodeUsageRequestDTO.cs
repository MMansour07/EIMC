using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class UpdateEGSCodeUsageRequestDTO: BaseDTO
    {
        public int codeUsageRequestId { get; set; }
        public string itemCode { get; set; }
        public string codeName { get; set; }
        public string codeNameAr { get; set; }
        public string description { get; set; }
        public string descriptionAr { get; set; }
        public string requestReason { get; set; }
        public string linkedCode { get; set; }
        public int parentCode { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime codactiveToeName { get; set; }
    }
}