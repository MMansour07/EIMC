using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class PrivilegeViewModel: BaseDTO
    {
        public IEnumerable<PrivilegeDTO> Privileges { get; set; }
        public IEnumerable<PermissionDTO> Permissions { get; set; }
    }
}
