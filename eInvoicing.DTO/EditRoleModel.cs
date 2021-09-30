using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class EditRoleModel: BaseDTO
    {
        public RoleDTO Role { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }
}
