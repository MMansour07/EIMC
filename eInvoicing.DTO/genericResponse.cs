using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class genericResponse
    {
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Message { get; set; }
        public dynamic Info { get; set; }
    }
}
