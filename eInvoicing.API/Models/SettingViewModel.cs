using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eInvoicing.API.Models
{
    public class SettingViewModel
    {
        public bool APIsEnvironment { get; set; }
        public bool ETAHubEnvironment { get; set; }
        public bool TypeVersion { get; set; }
        public string ProductionLoginURL { get; set; }
        public string ProductionApiURL { get; set; }
        public string ProductionClientId { get; set; }
        public string ProductionClientSecret { get; set; }
        public string ProductionAppKey { get; set; }
        public string ProductionAppId { get; set; }
        public string PreProductionLoginURL { get; set; }
        public string PreProductionApiURL { get; set; }
        public string PreProductionClientId { get; set; }
        public string PreProductionClientSecret { get; set; }
        public string PreProductionAppKey { get; set; }
        public string PreProductionAppId { get; set; }
        public string DevSignerURL { get; set; }
        public string ProductionSignerURL { get; set; }
    }
}