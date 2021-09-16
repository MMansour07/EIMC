using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class TAXABLEITEMSVM: BaseDTO
    {
        public string taxType { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public string subType { get; set; }
        
    }
}

