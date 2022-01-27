using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class RoleViewModel: BaseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PrivilegeDTO> Privileges { get; set; }
        public List<BusinessGroupDTO> BusinessGroups { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }
}
