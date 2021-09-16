using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class FieldItemDTO: BaseDTO
    {
        public int index { get; set; }
        public List<string> errors { get; set; }
    }
}
