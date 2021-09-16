using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class LoginResponseDTO: BaseDTO
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Privileges { get; set; }
        public List<string> Permissions { get; set; }
    }
}
