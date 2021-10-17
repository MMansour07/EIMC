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
    public class PermissionRepository : IdentityRepository<Permission>, IPermissionRepository
    {
        private readonly IdentityContext _context;
        public PermissionRepository(IdentityContext context) : base(context)
        {
            _context = context;
        }

    }
}

