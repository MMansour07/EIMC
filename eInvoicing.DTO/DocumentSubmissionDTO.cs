using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class DocumentSubmissionDTO : BaseDTO
    {
        public string submissionId { get; set; }
        public string statusCode { get; set; }
        public List<DocumentAcceptedDTO> acceptedDocuments { get; set; }
        public List<DocumentRejectedDTO> rejectedDocuments { get; set; }
    }
}
