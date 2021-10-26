using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using eInvoicing.DTO;
using eInvoicing.Web.Models;
using Newtonsoft.Json;

namespace eInvoicing.Web.Helper
{
    public class HttpClientHandler : IHttpClientHandler
    {
        private readonly IUserSession _userSession;
        public HttpClientHandler(IUserSession userSession)
        {
            _userSession = userSession;
        }
        public genericResponse GET(string url)
        {
            var response = new genericResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    client.BaseAddress = new Uri(_userSession.URL+url);
                    var postTask = Task.Run(() => client.GetAsync(_userSession.URL + url)).Result;
                    response.HttpStatusCode = postTask.StatusCode;
                    response.HttpResponseMessage = postTask;
                    if (postTask.IsSuccessStatusCode)
                    {
                        response.Info = postTask.Content.ReadAsStringAsync().Result;
                        response.Message = Global.Success.ToString();
                    }
                    else
                    { response.Message = Global.Failure.ToString(); }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                return response;
            }
        }

        public genericResponse POST(string url, object Content)
        {
            var response = new genericResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userSession.BearerToken);
                    client.BaseAddress = new Uri(_userSession.URL + url);
                    var postTask = Task.Run(() => client.PostAsJsonAsync(_userSession.URL + url, Content)).Result;
                    response.HttpStatusCode = postTask.StatusCode;
                    response.HttpResponseMessage = postTask;
                    if (postTask.IsSuccessStatusCode)
                    {
                        response.Info = postTask.Content.ReadAsStringAsync().Result;
                        response.Message = Global.Success.ToString();
                    }
                    else
                    response.Message = Global.Failure.ToString();
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                return response;
            }
        }
    }
}