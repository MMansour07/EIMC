using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DomainEntities.Base;

namespace eInvoicing.DomainEntities.Entities
{
    public class RolePrivilege: BaseEntity
    {
        public string RoleId { get; set; }
        public string PrivilegeId { get; set; }
        public Role Role { get; set; }
        public Privilege Privilege { get; set; }
        public virtual ICollection<RolePrivilegePermission> RolePrivilegePermissions { get; set; }

    }
}
