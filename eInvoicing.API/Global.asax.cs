using eInvoicing.API.Helper;
using eInvoicing.Service.Helper;
using Hangfire;
using Hangfire.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            Database.SetInitializer(new DBContextSeeder());

            //RecurringJob.AddOrUpdate(() => HangfireManager.SpecifyWhichActionsChain(), "0 0 10 * * ?");
            RecurringJob.AddOrUpdate(() => HangfireManager.GetReceivedDocuments(), "0 0 8 * * ?");
            RecurringJob.AddOrUpdate(() => HangfireManager.UpdateDocumentsStatusFromETAToEIMC(), Cron.Hourly());
            RecurringJob.AddOrUpdate(() => HangfireManager.RetrieveDocumentInvalidityReasons(), Cron.Hourly());
            RecurringJob.AddOrUpdate(() => HangfireManager.EIMCBackupPeriodically(), Cron.Daily);
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
