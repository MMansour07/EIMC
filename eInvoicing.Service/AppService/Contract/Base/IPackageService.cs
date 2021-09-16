using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface IPackageService
    {
        RequestDocumentPackageResponseDTO RequestDocumentPackage(RequestDocumentPackageRequestDTO obj, string Token, string URL);
        GetPackageRequestsRsDTO GetPackageRequests(GetPackageRequestsRqDTO obj, string Token, string URL);
        GetDocumentPackageResponseDTO GetDocumentPackage(GetDocumentPackageRequestDTO obj, string Token, string URL);
    }
}
