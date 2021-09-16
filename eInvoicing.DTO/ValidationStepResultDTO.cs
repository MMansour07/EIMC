using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class ValidationStepResultDTO
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public virtual ErrorDTO Error { get; set; }
    }
}
