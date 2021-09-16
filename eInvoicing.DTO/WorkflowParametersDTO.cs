using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class WorkflowParametersDTO
    {
        public int id { get; set; }
        public string parameter { get; set; }
        public decimal value { get; set; }
        public string activeFrom { get; set; }
        public string activeTo { get; set; }
    }
}
