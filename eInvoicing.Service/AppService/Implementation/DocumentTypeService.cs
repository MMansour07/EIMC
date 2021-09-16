using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using eInvoicing.Repository.Contract;
using eInvoicing.Repository.Contract.Base;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation.Base;
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
    public class DocumentTypeService : IDocumentTypeService
    {
        public ICollection<DocumentTypeDTO> GetDocumentTypes(string URL,string Key)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "documenttypes";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    { 
                        return JObject.Parse(result.Content.ReadAsStringAsync().Result).SelectToken("result").ToObject<List<DocumentTypeDTO>>();
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public DocumentTypeDTO GetDocumentType(string URL, string Key,int typeId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "documenttypes/"+ typeId;
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<DocumentTypeDTO>(result.Content.ReadAsStringAsync().Result);
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public DocumentTypeVersionDTO GetDocumentTypeVersion(string URL, string Key, int typeId, int versionId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "documenttypes/"+ typeId + "/versions/"+ versionId;
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<DocumentTypeVersionDTO>(result.Content.ReadAsStringAsync().Result);
                        return JsonConvert.DeserializeObject<DocumentTypeVersionDTO>(result.Content.ReadAsStringAsync().Result);
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
