using eInvoicing.API.Filters;
using eInvoicing.Service.AppService.Contract.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace eInvoicing.API.Controllers
{
    [Route("api/v0/documenttypes")]
    [JwtAuthentication]
    public class DocumentTypeController : ApiController
    {
        private readonly IDocumentTypeService _documentTypeService;
        private readonly IAuthService _auth;

        private readonly string loginUrl = ConfigurationManager.AppSettings["idSrvBaseUrl"];
        private readonly string APIUrl = ConfigurationManager.AppSettings["apiBaseUrl"];
        private readonly string client_id = ConfigurationManager.AppSettings["client_id"];
        private readonly string client_secret = ConfigurationManager.AppSettings["client_secret"];
        public DocumentTypeController() { }
        public DocumentTypeController(IDocumentTypeService documentTypeService, IAuthService auth)
        {
            _documentTypeService = documentTypeService;
            _auth = auth;
        }
        public IHttpActionResult GetDocumentTypes()
        {
            var auth = _auth.token(loginUrl, "client_credentials", client_id, client_secret, "InvoicingAPI");
            var result = _documentTypeService.GetDocumentTypes(APIUrl, auth.access_token);
            return Json(result);
        }
        public IHttpActionResult GetDocumentType(int typeId)
        {
            var auth = _auth.token(loginUrl, "client_credentials", client_id, client_secret, "InvoicingAPI");
            var result = _documentTypeService.GetDocumentType(APIUrl, auth.access_token, typeId);
            return Json(result);
        }
        public IHttpActionResult GetDocumentTypeVersion(int typeId, int versionId)
        {
            var auth = _auth.token(loginUrl, "client_credentials", client_id, client_secret, "InvoicingAPI");
            var result = _documentTypeService.GetDocumentTypeVersion(APIUrl, auth.access_token, typeId, versionId);
            return Json(result);
        }
    }
}