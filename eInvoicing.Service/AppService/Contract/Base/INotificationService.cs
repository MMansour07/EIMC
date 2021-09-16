using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface INotificationService
    {
        GetNotificationsResponse GetNotifications(string Url, string Key, int pageSize, int pageNo, string dateFrom, string dateTo, string Type, string Language, string status, string channel);
    }
}
