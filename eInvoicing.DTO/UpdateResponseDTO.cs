using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class UpdateResponseDTO : BaseDTO
    {
        public ErrorDTO error { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }
}