using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class DocumentRejectedDTO : BaseDTO
    {
        public string internalId { get; set; }
        public RejectedErrorDTO Error { get; set; }
    }
}
