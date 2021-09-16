using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class CustomErrorDTO : BaseDTO
    {
        public int id { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string target { get; set; }
        public string propertyPath { get; set; }
        public string DocumentId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<CustomErrorDTO> details { get; set; }

    }
}
