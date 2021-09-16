using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class SubmitDocumentRqDTO: BaseDTO
    {
        public string submittedBy { get; set; }
        public List<DocumentVM> documents { get; set; }
    }
}
