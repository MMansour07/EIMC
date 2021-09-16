using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class CreateEGSRequestDTO: BaseDTO
    {
        public List<EGSCodeDTO> items { get; set; }
    }
}
