using eInvoicing.DomainEntities.Entities;
using eInvoicing.Repository.Contract;
using eInvoicing.Repository.Contract.Base;
using eInvoicing.Repository.Implementation;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocumentRepository>().To<DocumentRepository>();
        }
    }
}
