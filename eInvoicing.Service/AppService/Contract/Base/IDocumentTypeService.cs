using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface IDocumentTypeService 
    {
        ICollection<DocumentTypeDTO> GetDocumentTypes(string URL, string Key);
        DocumentTypeDTO GetDocumentType(string URL, string Key, int typeId);
        DocumentTypeVersionDTO GetDocumentTypeVersion(string URL, string Key, int typeId, int versionId);
    }
}
