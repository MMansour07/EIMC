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
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        private readonly ApplicationContext _context;
        public UserRoleRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        public new void DeleteRange(string Id)
        {
            var entities = DbSet.Where(e => e.UserId == Id);
            DbSet.RemoveRange(entities);
        }

    }
}
