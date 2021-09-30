using AutoMapper;
using eInvoicing.API.Models;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.Repository.Contract;
using eInvoicing.Repository.Contract.Base;
using eInvoicing.Repository.Implementation;
using eInvoicing.Repository.Implementation.Base;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation;
using Ninject;
using Ninject.Extensions.ChildKernel;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace eInvoicing.API.Infrastructure
{
    public class NinjectResolver: IDependencyResolver
    {
        private IKernel kernel;

        public NinjectResolver() : this(new StandardKernel())
        {

        }

        public NinjectResolver(IKernel ninjectKernel, bool scope = false)
        {
            kernel = ninjectKernel;
            if (!scope)
            {
                AddBindings(kernel);
            }
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectResolver(AddRequestBindings(new ChildKernel(kernel)), true);
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public void Dispose()
        {

        }

        private void AddBindings(IKernel kernel)
        {
            // singleton and transient bindings go here
            //kernel.Bind<IDocumentSubmissionsService>().To<IDocumentSubmissionsService>();
            //kernel.Bind<IAuthService>().To<AuthService>();
        }

        private IKernel AddRequestBindings(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope();
            kernel.Bind<IRepository<User>>().To<Repository<User>>().InThreadScope();
            kernel.Bind<IDocumentRepository>().To<DocumentRepository>().InThreadScope();
            kernel.Bind<IAuthRepository>().To<AuthRepository>().InThreadScope();
            kernel.Bind<IRoleRepository>().To<RoleRepository>().InThreadScope();
            kernel.Bind<IPermissionRepository>().To<PermissionRepository>().InThreadScope();
            kernel.Bind<IRolePermissionRepository>().To<RolePermissionRepository>().InThreadScope();
            kernel.Bind<IUserRoleRepository>().To<UserRoleRepository>().InThreadScope();
            kernel.Bind<IInvoiceLineRepository>().To<InvoiceLineRepository>().InThreadScope();
            kernel.Bind<ITaxableItemRepository>().To<TaxableItemRepository>().InThreadScope();
            kernel.Bind<ILookupRepository>().To<LookupRepository>().InThreadScope();
            kernel.Bind<IErrorReposistory>().To<ErrorRepository>().InThreadScope();
            kernel.Bind<IDocumentService>().To<DocumentService>().InThreadScope();
            kernel.Bind<IReportService>().To<ReportService>().InThreadScope();
            kernel.Bind<IErrorService>().To<ErrorService>().InThreadScope();
            kernel.Bind<IUserSession>().To<UserSession>().InThreadScope();
            kernel.Bind<IAuthService>().To<AuthService>().InThreadScope();
            kernel.Bind<IInternalAuthService>().To<InternalAuthService>().InThreadScope();
            kernel.Bind<IRoleService>().To<RoleService>().InThreadScope();
            kernel.Bind<IMapper>().To<Mapper>().InThreadScope();
            kernel.Bind<ISyncService>().To<SyncService>().InThreadScope();
            kernel.Bind<ILookupService>().To<LookupService>().InThreadScope();
            return kernel;
        }
    }
}