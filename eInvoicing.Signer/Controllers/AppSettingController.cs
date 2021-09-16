using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using eInvoicing.API.Filters;
using eInvoicing.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace eInvoicing.API.Controllers
{
    [HMACAuthentication]
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingController : ControllerBase
    {
        [HttpPost]
        [Route("Update")]
        public bool Update(SettingViewModel model)
        {
            try
            {
                //_configuration.GetSection("AppSettings").GetSection("AppId").Value = model.PreProductionAppId;
                //_configuration.GetSection("AppSettings").GetSection("AppKey").Value = model.PreProductionAppKey;
                //_configuration.GetSection("AppSettings").GetSection("ProdAppId").Value = model.ProductionAppId;
                //_configuration.GetSection("AppSettings").GetSection("ProdAppKey").Value = model.ProductionAppKey;
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
