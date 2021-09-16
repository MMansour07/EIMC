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
using System.Net.Http.Headers;
using System.Net;

namespace eInvoicing.Service.AppService.Implementation
{
    public class PackageService : IPackageService
    {
        public PackageService()
        {

        }

        public GetDocumentPackageResponseDTO GetDocumentPackage(GetDocumentPackageRequestDTO obj, string Token, string URL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "documentPackages/:"+obj.rid;
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return new GetDocumentPackageResponseDTO() { statusCode = result.StatusCode };
                    }
                    return new GetDocumentPackageResponseDTO() { error = JsonConvert.DeserializeObject<GetDocumentPackageResponseDTO>(result.Content.ReadAsStringAsync().Result).error };
                }
            }
            catch (Exception ex)
            {
                return new GetDocumentPackageResponseDTO() { error = new ErrorDTO() { message = ex.Message.ToString() }, statusCode = HttpStatusCode.InternalServerError};

            }
        }

        public GetPackageRequestsRsDTO GetPackageRequests(GetPackageRequestsRqDTO obj, string Token, string URL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "documentPackages/requests?pageSize="+obj.pageSize+"&pageNo="+obj.pageNo;
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<GetPackageRequestsRsDTO>(result.Content.ReadAsStringAsync().Result);
                    }
                    return new GetPackageRequestsRsDTO() { error = JsonConvert.DeserializeObject<GetPackageRequestsRsDTO>(result.Content.ReadAsStringAsync().Result).error};
                }
            }
            catch (Exception ex)
            {
                return new GetPackageRequestsRsDTO() { error = new ErrorDTO() { message = ex.Message.ToString() } };
            }
        }

        public RequestDocumentPackageResponseDTO RequestDocumentPackage(RequestDocumentPackageRequestDTO obj, string Token, string URL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "documentPackages/requests";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PostAsJsonAsync(URL, obj);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var res = JsonConvert.DeserializeObject<RequestDocumentPackageResponseDTO>(result.Content.ReadAsStringAsync().Result);
                        return new RequestDocumentPackageResponseDTO() { statusCode = result.StatusCode, rid = res.rid};
                    }
                    var response = JsonConvert.DeserializeObject<RequestDocumentPackageResponseDTO>(result.Content.ReadAsStringAsync().Result);
                    response.statusCode = result.StatusCode;
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new RequestDocumentPackageResponseDTO() { statusCode = HttpStatusCode.InternalServerError, error = new ErrorDTO() { message = ex.Message.ToString()} };
            }
        }
    }
}
