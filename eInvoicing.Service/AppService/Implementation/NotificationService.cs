using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Implementation
{
    public class NotificationService : INotificationService
    {
        public GetNotificationsResponse GetNotifications(string Url, string Key, int pageSize, int pageNo, string dateFrom, string dateTo, string type, string language, string status, string channel)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Url += "notifications/taxpayer?dateFrom="+dateFrom+"&dateTo="+dateTo+"&type="+type+"&language="+language+"&status="+status+"&channel="+channel+"&pageNo="+pageNo+"&pageSize="+pageSize;
                client.BaseAddress = new Uri(Url);
                var postTask = client.GetAsync(Url);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<GetNotificationsResponse>(result.Content.ReadAsStringAsync().Result);
                }
                return null;
            }
        }

    }
}
