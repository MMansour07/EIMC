using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;
using Newtonsoft.Json;

namespace eInvoicing.DTO
{
    public class UserDTO: BaseDTO
    {
        public string Token { get; set; }
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string BusinessGroup { get; set; }
        public string BusinessGroupId { get; set; }
        public string RIN { get; set; }
        public bool IsDBSync { get; set; }

        public string PhoneNumber { get; set; }
        public List<RoleDTO> Roles { get; set; }
        [JsonProperty("Privileges")]
        public List<PrivilegeDTO> Privileges { get; set; }
        [JsonProperty("Permissions")]

        public List<PermissionDTO> Permissions { get; set; }
        public List<string> stringfiedRoles
        {
            get
            {
                return this.Roles.Select(x => x.Name).ToList();
            }
        }
        public List<string> stringfiedPermissions
        {
            get
            {
                return this.Permissions.Select(x => x.Action).ToList();
            }
        }
    }
    public class PrivilegeDTO : BaseDTO
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Controller")]

        public string Controller { get; set; }
    }
    public class PermissionDTO : BaseDTO
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Action")]
        public string Action { get; set; }
    }
}
