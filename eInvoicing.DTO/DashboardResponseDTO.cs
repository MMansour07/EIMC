using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class DashboardResponseDTO : BaseDTO
    {
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }
        public DashboardDTO Data { get; set; }
    }
}
