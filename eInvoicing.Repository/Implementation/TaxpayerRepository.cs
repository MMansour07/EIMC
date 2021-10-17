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
    public class TaxpayerRepository : IdentityRepository<TaxPayer>, ITaxpayerRepository
    {
        private readonly IdentityContext _context;
        public TaxpayerRepository(IdentityContext context) : base(context)
        {
            _context = context;
        }
        public TaxPayer GetLastRecord()
        {
            try
            {
                return _context.TaxPayers.AsNoTracking().OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.Status == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
