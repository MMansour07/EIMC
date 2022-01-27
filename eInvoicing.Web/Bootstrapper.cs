using System.Web.Mvc;
using eInvoicing.Web.Controllers;
using eInvoicing.Web.Helper;
using eInvoicing.Web.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.Unity;
using Unity.Mvc3;


namespace eInvoicing.Web
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IUserSession, UserSession>();
            //container.RegisterType<ILogger<AccountController>, Logger<AccountController>>();
            container.RegisterType<IHttpClientHandler, HttpClientHandler>();
            //container.RegisterType<DbContext, ApplicationDbContext>(
            //new HierarchicalLifetimeManager());
            //container.RegisterType<UserManager<ApplicationUser>>(
            //    new HierarchicalLifetimeManager());
            //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
            //    new HierarchicalLifetimeManager());
            //container.RegisterType<AccountController>(
            //    new InjectionConstructor());
            //container.RegisterType<IAuthenticationManager>(
            //new InjectionFactory(o => System.Web.HttpContext.Current.GetOwinContext().Authentication));
            return container;
        }
    }
}