using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class AuthDTO: BaseDTO
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public decimal expires_in { get; set; }
        public string scope { get; set; }
    }
}
