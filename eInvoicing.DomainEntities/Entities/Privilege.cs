using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DomainEntities.Base;

namespace eInvoicing.DomainEntities.Entities
{
    public class Privilege : BaseEntity
    {
        public string Controller { get; set; }
        public virtual ICollection<RolePrivilege> RolePrivileges { get; set; }
        public virtual ICollection<RolePrivilegePermission> PrivilegePermission { get; set; }
    }
}
