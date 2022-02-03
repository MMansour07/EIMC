using eInvoicing.DomainEntities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DomainEntities.Entities
{
    public class StepError : BaseEntity
    {
        public string PropertyName { get; set; }
        public string PropertyPath { get; set; }
        public string ErrorCode { get; set; }
        public string Error { get; set; }
        public string ErrorAr { get; set; }
        public string ValidationStepId { get; set; }
        public string ErrorId { get; set; }
        public virtual StepError _StepError { get; set; }

        [ForeignKey("ErrorId")]
        public virtual List<StepError> InnerError { get; set; }

        [ForeignKey("ValidationStepId")]
        public virtual ValidationStep ValidationStep { get; set; }
    }
}
