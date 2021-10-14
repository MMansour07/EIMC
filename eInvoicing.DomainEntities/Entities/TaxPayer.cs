using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DomainEntities.Base;

namespace eInvoicing.DomainEntities.Entities
{
    public class TaxPayer : BaseEntity
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string ERPEn { get; set; }
        public string ERPAr { get; set; }
        public string RegistrationNumber { get; set; }
        public string token { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime ClientSecretExpDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string LicenseType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int IRN { get; set; }
        public bool Status { get; set; }
    }
}
