using eInvoicing.DomainEntities.Entities;
using eInvoicing.Persistence;
using eInvoicing.Repository.Contract;
using eInvoicing.Repository.Implementation.Base;
using System;
using System.Linq;

namespace eInvoicing.Repository.Implementation
{
    public class ValidationStepRepository : Repository<ValidationStep>, IValidationStepRepository
    {
        public ValidationStepRepository(ApplicationContext context) : base(context)
        {
            
        }
        public bool CheckReasonsInsertedBefore(string DocumentId)
        {
            try
            {
                return DbSet.AsNoTracking().Any(x => x.DocumentId == DocumentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
