using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using eInvoicing.DomainEntities.Base;

namespace eInvoicing.DomainEntities.Entities
{
    [Table("BusinessGroups")]
    public class BusinessGroup : BaseEntity
    {
        public string GroupName { get; set; }
        public string BusinessType { get; set; }
        public string SyncType { get; set; }
        public string Token { get; set; }
        public string USB_SerialNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsDBSync { get; set; }
        public string Desc { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<BusinessGroup> BusinessGroups { get; set; }
    }
}
