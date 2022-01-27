using eInvoicing.API.Models;
using eInvoicing.DTO;
using eInvoicing.Persistence;
using eInvoicing.Repository.Contract;
using eInvoicing.Repository.Implementation;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation;
using Hangfire;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace eInvoicing.API.Helper
{
    public class HangfireManager
    {
        public static string GetBaseURL()
        {
            Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
            if (objAppsettings != null)
            {
                if (objAppsettings.Settings["Mode"].Value.ToLower() == "dev")
                    return objAppsettings.Settings["IISExpress_URL"].Value;
                else
                    return objAppsettings.Settings["IIS_URL"].Value;
            }
            return null;
        }
        public static void SpecifyWhichActionsChain()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = TimeSpan.FromMinutes(60);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = GetBaseURL() + "/api/document/SpecifyWhichActionsChain";
                    client.BaseAddress = new Uri(url);
                    var postTask = client.GetAsync(url);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void UpdateDocumentsStatusFromETAToEIMC()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = TimeSpan.FromMinutes(60);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = GetBaseURL() + "/api/document/updatedocumentsStatusfromETAToEIMC";
                    client.BaseAddress = new Uri(url);
                    var postTask = client.GetAsync(url);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                       
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void RetrieveDocumentInvalidityReasons()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = TimeSpan.FromMinutes(60);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = GetBaseURL() + "/api/document/RetrieveDocumentInvalidityReasons";
                    client.BaseAddress = new Uri(url);
                    var postTask = client.GetAsync(url);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void EIMCBackupPeriodically()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.Timeout = TimeSpan.FromMinutes(60);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = GetBaseURL() + "/api/document/EIMCBackupPeriodically";
                    client.BaseAddress = new Uri(url);
                    var postTask = client.GetAsync(url);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}