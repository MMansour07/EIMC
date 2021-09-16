using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface IDocumentSubmissionsService
    {
        DocumentSubmissionDTO documentsubmissions(List<DocumentVM> documents, string Token, string URL);
        DocumentSubmissionDTO documentautosubmissions(string Token, string URL);
    }
}
