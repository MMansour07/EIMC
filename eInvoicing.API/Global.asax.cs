using eInvoicing.API.Helper;
using eInvoicing.API.Infrastructure;
using eInvoicing.Persistence;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation;
using eInvoicing.Service.Helper;
using Hangfire;
using Hangfire.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace eInvoicing.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfiguration.Init();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HangfireAspNet.Use(GetHangfireServers);

            RecurringJob.AddOrUpdate(() => HangfireManager.Sync(), Cron.Daily);
            // update Documents with the status retreived from ETA
            RecurringJob.AddOrUpdate(() => HangfireManager.SyncFromETAToDB(), Cron.Daily);
            //RecurringJob.AddOrUpdate(() => HangfireManager.AutoSubmission(), Cron.Daily);
        }
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            Hangfire.GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage("db_connection", new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }
    }
}
