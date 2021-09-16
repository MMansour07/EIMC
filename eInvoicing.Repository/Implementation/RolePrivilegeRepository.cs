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
    public class RolePrivilegeRepository : Repository<RolePrivilege>, IRolePrivilegeRepository
    {
        private readonly ApplicationContext _context;
        public RolePrivilegeRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        public new void DeleteRange(string RoleId)
        {
            var entities = _context.RolePrivileges.Where(e => e.RoleId == RoleId);
            DbSet.RemoveRange(entities);
        }

    }
}
