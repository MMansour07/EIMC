using eInvoicing.DomainEntities.Entities;
using eInvoicing.Repository.Contract.Base;

namespace eInvoicing.Repository.Contract
{
    public interface IValidationStepRepository : IRepository<ValidationStep>
    {
        bool CheckReasonsInsertedBefore(string DocumentId);
    }
}
