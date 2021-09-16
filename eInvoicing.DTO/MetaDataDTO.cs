using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class MetaDataDTO: BaseDTO
    {
        public int totalPages { get; set; }
        public int totalCount { get; set; }
    }
}
