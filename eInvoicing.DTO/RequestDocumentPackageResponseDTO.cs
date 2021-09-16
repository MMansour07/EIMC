using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class RequestDocumentPackageResponseDTO : BaseDTO
    {
        public string rid { get; set; }
        public ErrorDTO error { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }
}