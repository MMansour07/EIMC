using eInvoicing.Service.AppService.Contract.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace eInvoicing.API.Controllers
{
    [Route("api/v0/notifications")]
    public class NotificationController : ApiController
    {
        private readonly INotificationService _notificationService;
        private readonly IAuthService _auth;

        private readonly string loginUrl = ConfigurationManager.AppSettings["idSrvBaseUrl"];
        private readonly string APIUrl = ConfigurationManager.AppSettings["apiBaseUrl"];
        private readonly string client_id = ConfigurationManager.AppSettings["client_id"];
        private readonly string client_secret = ConfigurationManager.AppSettings["client_secret"];
        public NotificationController() { }
        public NotificationController(INotificationService notificationService, IAuthService auth)
        {
            _notificationService = notificationService;
            _auth = auth;
        }
        public IHttpActionResult GetNotifications(int pageSize, int pageNo, string dateFrom, string dateTo, string type, string language, string status, string channel)
        {
            var auth =  _auth.token(loginUrl, "client_credentials", client_id, client_secret, "InvoicingAPI");
            var result = _notificationService.GetNotifications(APIUrl, auth.access_token, pageSize, pageNo, dateFrom, dateTo, type, language, status, channel);
            return Json(result);
        }
    }
}