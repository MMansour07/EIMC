using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace eInvoicing.Signer.Models
{
    public class CommonResponse
    {
        public string Message { get; set; } = "Operation has been executed successfully!";
        public HttpStatusCode Status { get; set; }
        public dynamic Info { get; set; }
    }
}
