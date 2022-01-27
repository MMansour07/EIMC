using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;
using Newtonsoft.Json;

namespace eInvoicing.DTO
{
    public class BusinessGroupDTO : BaseDTO
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public string BusinessType { get; set; }
        public string SyncType { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Token { get; set; }
        public string Address { get; set; }
        public bool IsDBSync { get; set; }
        public string Desc { get; set; }
    }
}