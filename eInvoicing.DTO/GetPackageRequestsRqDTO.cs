using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class GetPackageRequestsRqDTO: BaseDTO
    {
        public int pageNo { get; set; }
        public int pageSize { get; set; }
    }
}
