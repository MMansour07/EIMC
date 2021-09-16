using System;
using System.Collections.Generic;
using System.Text;

namespace eInvoicing.DomainEntities.Base
{
    public interface IEntity
    {
        string Id { get; set; }
        string CreatedBy { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        DateTime? ModifiedOn { get; set; }
        bool? IsDeleted { get; set; }
    }
}
