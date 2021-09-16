using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class DocumentTypeDTO : BaseDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string activeFrom { get; set; }
        public string activeTo { get; set; }
        public List<DocumentTypeVersionDTO> documentTypeVersions { get; set; }
        public ICollection<WorkflowParametersDTO> documentWorkflowParams { get; set; }
    }
}
