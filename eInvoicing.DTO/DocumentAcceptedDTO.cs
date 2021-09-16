using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class DocumentAcceptedDTO
    {
        public string uuid { get; set; }
        public string longId { get; set; }
        public string internalId { get; set; }
        public string hashKey { get; set; }
    }
}
