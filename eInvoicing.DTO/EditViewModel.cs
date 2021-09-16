using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class EditViewModel: BaseDTO
    {
        public EditModelDTO User { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
