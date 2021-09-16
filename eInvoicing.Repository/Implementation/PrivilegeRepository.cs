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
    public class PrivilegeRepository : Repository<Privilege>, IPrivilegeRepository
    {
        private readonly ApplicationContext _context;
        public PrivilegeRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

    }
}
