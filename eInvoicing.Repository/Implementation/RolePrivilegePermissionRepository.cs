using eInvoicing.DomainEntities.Entities;
using eInvoicing.Persistence;
using eInvoicing.Repository.Contract;
using eInvoicing.Repository.Implementation.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Repository.Implementation
{
    public class RolePrivilegePermissionRepository : Repository<RolePrivilegePermission>, IRolePrivilegePermissionRepository
    {
        private readonly ApplicationContext _context;
        public RolePrivilegePermissionRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        public new void DeleteRange(string RolePrivilegeId)
        {
            var entities = _context.RolePrivilegePermissions.Where(e => e.RolePrivilegeId == RolePrivilegeId);
            DbSet.RemoveRange(entities);
        }

    }
}

