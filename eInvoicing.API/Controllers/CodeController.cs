using System;
using System.Net;
using System.Web.Http;
using eInvoicing.API.Filters;
using eInvoicing.API.Models;
using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;

namespace eInvoicing.API.Controllers
{
    [JwtAuthentication]
    public class CodeController : BaseController
    {
        private readonly ICodeService _codeService;
        private readonly IAuthService _auth;
        private readonly IUserSession _userSession;

        public CodeController(ICodeService codeService, IAuthService auth, IUserSession userSession)
        {
            _codeService = codeService;
            _auth = auth;
            _userSession = userSession;
        }

        [HttpPost]
        [Route("api/code/SearchMyEGSCodeUsageRequests")]
        public IHttpActionResult SearchMyEGSCodeUsageRequests(SearchEGSCodeRequestDTO req)
        {
            try
            {
                _userSession.GetBusinessGroupId(this.GetBusinessGroupId());
                var auth = _auth.token(_userSession.loginUrl, "client_credentials", _userSession.client_id, _userSession.client_secret, "InvoicingAPI");
                var result = _codeService.SearchMyEGSCodeUsageRequests(req, auth.access_token, _userSession.submissionurl);
                if (result != null && result.StatusCode == HttpStatusCode.OK)
                    return Ok(result);
                else if (result != null && result.StatusCode == HttpStatusCode.Unauthorized)
                    return Unauthorized();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
