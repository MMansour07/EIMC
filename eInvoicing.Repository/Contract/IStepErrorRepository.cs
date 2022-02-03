using System.Collections.Generic;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.Repository.Contract.Base;

namespace eInvoicing.Repository.Contract
{
    public interface IStepErrorRepository : IRepository<StepError>
    {
        void DeleteByValidationStepId(List<string> ValidationStepIds);
    }
}
