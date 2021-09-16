using eInvoicing.DomainEntities.Entities;
using eInvoicing.Repository.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Repository.Contract
{
    public interface IAuthRepository : IRepository<User>
    {
        new User Get(string id);
        User GetById(string Id);
    }
}
