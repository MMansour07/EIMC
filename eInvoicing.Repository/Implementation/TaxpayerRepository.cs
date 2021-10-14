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
    public class TaxpayerRepository : Repository<TaxPayer>, ITaxpayerRepository
    {
        public TaxpayerRepository(ApplicationContext context) : base(context)
        {

        }
        public TaxPayer GetLastRecord()
        {
            try
            {
                return DbSet.AsNoTracking().OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.Status == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
