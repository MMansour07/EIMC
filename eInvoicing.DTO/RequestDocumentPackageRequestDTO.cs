using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class RequestDocumentPackageRequestDTO : BaseDTO
    {
        public string type { get; set; }
        public string format { get; set; }
        public QueryParameters queryParameters { get; set; }
        public string receiverSenderId { get; set; }
        public int receiverSenderType { get; set; }
        public List<string> productsInternalCodes { get; set; }
        public List<string> documentTypeNames { get; set; }
    }
}