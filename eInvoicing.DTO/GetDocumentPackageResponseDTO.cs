using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class GetDocumentPackageResponseDTO: BaseDTO
    {
        public ErrorDTO error { get; set; }
        public HttpStatusCode statusCode { get; set; }

    }
}
