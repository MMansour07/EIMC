using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class QueryParameters: BaseDTO
    {
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public List<string> statuses { get; set; }
        public string documentTypeName { get; set; }
        public string productsInternalCodes { get; set; }
        public string receiverSenderType { get; set; }
        public string receiverSenderId { get; set; }
    }
}
