using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class RequestCodeReuseRequestDTO : BaseDTO
    {
        public List<RequestCodeReuseItemsDTO> items { get; set; }
    }
}