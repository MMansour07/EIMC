using eInvoicing.DomainEntities.Entities;
using eInvoicing.Repository.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Repository.Contract
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        new void DeleteRange(string Id);
    }
}
