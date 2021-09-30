using eInvoicing.DomainEntities.Entities;
using eInvoicing.Repository.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Repository.Contract
{
    public interface IRolePermissionRepository : IRepository<RolePermission>
    {
        new void DeleteRange(string RolePrivilegeId);
    }
}
