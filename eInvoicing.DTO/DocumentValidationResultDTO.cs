using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class DocumentValidationResultDTO: BaseDTO
    {
        public string status { get; set; }
        public List<ValidationStepResultDTO> validationSteps { get; set; }
    }
}
