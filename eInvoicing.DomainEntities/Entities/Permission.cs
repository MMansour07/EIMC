using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DomainEntities.Base;

namespace eInvoicing.DomainEntities.Entities
{
    public class Permission: BaseEntity
    {
        public string Action { get; set; }
        public virtual ICollection<RolePermission> RolePermission { get; set; }
    }
}
