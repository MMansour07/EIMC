using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class LicenseDTO: BaseDTO
    {
        public int Id { get; set; }
        public string License { get; set; }
        public string LicenseType { get; set; }
    }
}
