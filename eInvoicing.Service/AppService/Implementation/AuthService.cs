using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using eInvoicing.Repository.Contract.Base;
using eInvoicing.DomainEntities.Entities;
using Ninject;
using eInvoicing.Repository.Contract;
using System.Reflection;
using eInvoicing.Repository.Implementation;

namespace eInvoicing.Service.AppService.Implementation
{
    public class AuthService: IAuthService
    {
        public AuthDTO token(string URL, string grant_type, string client_id, string client_secret, string scope)
        {
            try
            {
                var FormUrlEncodedContent = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", grant_type),
                    new KeyValuePair<string, string>("client_id", client_id),
                    new KeyValuePair<string, string>("client_secret", client_secret),
                    new KeyValuePair<string, string>("scope", scope)
                };
                using (HttpClient client = new HttpClient())
                {
                    using (var content = new FormUrlEncodedContent(FormUrlEncodedContent))
                    {
                        content.Headers.Clear();
                        content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        URL += "connect/token";
                        client.BaseAddress = new Uri(URL);
                        var postTask = client.PostAsync(URL, content);
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return JsonConvert.DeserializeObject<AuthDTO>(result.Content.ReadAsStringAsync().Result);
                        }
                        return new AuthDTO() { /*Response = result*/ };
                    }
                }
            }
            catch (Exception ex)
            {
                return new AuthDTO() { /*ErrorMessage = ex.Message.ToString()*/ };
            }
        }
    }
}
