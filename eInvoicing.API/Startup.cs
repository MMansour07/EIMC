using eInvoicing.API.Helper;
using Hangfire;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(eInvoicing.API.Startup))]

namespace eInvoicing.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseHangfireDashboard();
            //Database.SetInitializer(new DBContextSeeder());

        }
    }
}
