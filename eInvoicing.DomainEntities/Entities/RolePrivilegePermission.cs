using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DomainEntities.Base;

namespace eInvoicing.DomainEntities.Entities
{
    public class RolePrivilegePermission : BaseEntity
    {
        public string RolePrivilegeId { get; set; }
        public string PermissionId { get; set; }
        public RolePrivilege RolePrivilege { get; set; }
        public Permission Permission { get; set; }
    }
}
