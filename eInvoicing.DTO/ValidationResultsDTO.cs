using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class ValidationResultsDTO
    {
        public string status { get; set; }
        public List<ValidationStepResultDTO> validationSteps { get; set; }
    }
}
