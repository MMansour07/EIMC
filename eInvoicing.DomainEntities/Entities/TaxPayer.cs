using System;
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
        public string PreProdClientId { get; set; }
        public string PreProdClientSecret { get; set; }
        public string ProdClientId { get; set; }
        public string ProdClientSecret { get; set; }
        public string TokenPin { get; set; }
        public int IRN { get; set; }
        public string BusinessGroupId { get; set; }
        public bool Status { get; set; }
        public virtual BusinessGroup BusinessGroup { get; set; }

    }
}
