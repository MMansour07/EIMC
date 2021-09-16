using eInvoicing.DomainEntities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Repository.Contract.Base
{
    public interface IUnitOfWork : IDisposable
    {
        T Save<T>(T entity) where T : IEntity;
        void Save();
        Task SaveAsync();
    }
}
