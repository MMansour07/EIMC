using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace eInvoicing.Service.AppService.Implementation
{
    public class CodeService: ICodeService
    {
        public CodeService()
        {
        }
        public CreateEGSResponseDTO CreateEGSCodeUsage(CreateEGSRequestDTO obj, string Token, string URL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "codetypes/requests/codes";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PostAsJsonAsync(URL, obj);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<CreateEGSResponseDTO>(result.Content.ReadAsStringAsync().Result);
                        response.statusCode = result.StatusCode;
                        return response;
                    }
                    return new CreateEGSResponseDTO() { statusCode = result.StatusCode};
                }
            }
            catch (Exception ex)
            {
                return new CreateEGSResponseDTO() { statusCode = HttpStatusCode.InternalServerError };
            }
        }

        public GetCodeDetailsbyItemCodeResultDTO GetCodeDetailsbyItemCode(GetCodeDetailsbyItemCodeRequestDTO obj, string Token, string URL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "codetypes/:"+obj.codeType+"/codes/:"+obj.itemCode;
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<GetCodeDetailsbyItemCodeResultDTO>(result.Content.ReadAsStringAsync().Result);
                    }
                    return new GetCodeDetailsbyItemCodeResultDTO() { /*Response = result */};
                }
            }
            catch (Exception ex)
            {
                return new GetCodeDetailsbyItemCodeResultDTO() { /*ErrorMessage = ex.Message.ToString()*/ };
            }
        }

        public GetCodeDetailsbyItemCodeResponseDTO GetCodeDetailsbyItemCodelst(GetCodeDetailsbyItemCodeRequestDTO obj, string Token, string URL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "codetypes/:" + obj.codeType + "/codes/:null";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<GetCodeDetailsbyItemCodeResponseDTO>(result.Content.ReadAsStringAsync().Result);
                    }
                    return new GetCodeDetailsbyItemCodeResponseDTO() { /*Response = result */};
                }
            }
            catch (Exception ex)
            {
                return new GetCodeDetailsbyItemCodeResponseDTO() { /*ErrorMessage = ex.Message.ToString()*/ };
            }
        }

        public CreateEGSResponseDTO RequestCodeReuse(RequestCodeReuseRequestDTO obj, string Token, string URL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "codetypes/requests/codeusages";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PutAsJsonAsync(URL, obj);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<CreateEGSResponseDTO>(result.Content.ReadAsStringAsync().Result);
                        response.statusCode = result.StatusCode;
                        return response;
                    }
                    return new CreateEGSResponseDTO() { statusCode = result.StatusCode };
                }
            }
            catch (Exception ex)
            {
                return new CreateEGSResponseDTO() { statusCode = HttpStatusCode.InternalServerError };
            }
        }

        public SearchEGSCodeResponseDTO SearchMyEGSCodeUsageRequests(SearchEGSCodeRequestDTO obj, string Key, string URL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = TimeSpan.FromMinutes(60);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Key);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "codetypes/requests/my";
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var res = JsonConvert.DeserializeObject<SearchEGSCodeResponseDTO>(result.Content.ReadAsStringAsync().Result);
                        res.StatusCode = HttpStatusCode.OK;
                        return res;
                    }
                    return new SearchEGSCodeResponseDTO() { StatusCode = result.StatusCode };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SearchPublishedCodesResponseDTO SearchPublishedCodes(SearchPublishedCodesRequestDTO obj, string Token, string URL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "codetypes/:"+obj.codeType+"/codes?ParentLevelName="+obj.parentLevelName+"&OnlyActive="+obj.onlyActive+"&ActiveFrom="+obj.activeFrom+"&Ps="+obj.ps+"&Pn="+obj.pn+"&OrdDir="+obj.orderDirections+"&CodeTypeLevelNumber="+obj.codeTypeLevelNumber;
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.GetAsync(URL);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<SearchPublishedCodesResponseDTO>(result.Content.ReadAsStringAsync().Result);
                    }
                    return new SearchPublishedCodesResponseDTO() { /*Response = result */};
                }
            }
            catch (Exception ex)
            {
                return new SearchPublishedCodesResponseDTO() { /*ErrorMessage = ex.Message.ToString()*/ };
            }
        }

        public UpdateResponseDTO UpdateCode(UpdateCodeRequestDTO obj, string Token, string URL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "codetypes/:"+obj.codeType+"/codes/:"+obj.itemCode;
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PutAsJsonAsync(URL, obj);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return new UpdateResponseDTO() { statusCode = result.StatusCode };
                    }
                    var response = JsonConvert.DeserializeObject<UpdateResponseDTO>(result.Content.ReadAsStringAsync().Result);
                    response.statusCode = result.StatusCode;
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new UpdateResponseDTO() { statusCode = HttpStatusCode.InternalServerError };
            }
        }

        public UpdateResponseDTO UpdateEGSCodeUsage(UpdateEGSCodeUsageRequestDTO obj, string Token, string URL)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    URL += "codetypes/requests/codes/:"+obj.codeUsageRequestId;
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PutAsJsonAsync(URL, obj);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return new UpdateResponseDTO() { statusCode = result.StatusCode };
                    }
                    var response = JsonConvert.DeserializeObject<UpdateResponseDTO>(result.Content.ReadAsStringAsync().Result);
                    response.statusCode = result.StatusCode;
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new UpdateResponseDTO() { statusCode = HttpStatusCode.InternalServerError };
            }
        }
    }
}
