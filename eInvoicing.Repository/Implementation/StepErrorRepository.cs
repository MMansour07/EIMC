using eInvoicing.DomainEntities.Entities;
using eInvoicing.Persistence;
using eInvoicing.Repository.Contract;
using eInvoicing.Repository.Implementation.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eInvoicing.Repository.Implementation
{
    public class StepErrorRepository : Repository<StepError>, IStepErrorRepository
    {
        public StepErrorRepository(ApplicationContext context) : base(context)
        {
            
        }
        public void DeleteByValidationStepId(List<string> ValidationStepIds)
        {
            try
            {
                var entities = DbSet.Where(s => ValidationStepIds.Contains(s.ValidationStepId)).SelectMany(s => s.InnerError);
                if (entities.Count() > 0)
                {
                    var RemovedEntity = DbSet.RemoveRange(entities);
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
