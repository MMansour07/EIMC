using eInvoicing.DomainEntities.Entities;
using eInvoicing.Persistence;
using eInvoicing.Repository.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Repository.Contract
{
    public interface IRolePrivilegeRepository : IRepository<RolePrivilege>
    {
        void DeleteRange(string RoleId);
    }
}
