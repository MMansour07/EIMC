using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class TaxpayerDTO : BaseDTO
    {
        public int Id { get; set; }
        public string TaxPayerNameEn { get; set; }
        public string TaxPayerNameAr { get; set; }
        public string ERPName { get; set; }
        public string ERPAr { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime LicenseCreationDate { get; set; }
        public DateTime LicenseExpirationDate { get; set; }
        public DateTime ClientSecretExpirationDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string LicenseType { get; set; }
        public string PIN { get; set; }
        public string SRN { get; set; }
        public string PreProdClientId { get; set; }
        public string PreProdClientSecret { get; set; }
        public string ProdClientId { get; set; }
        public string ProdClientSecret { get; set; }
        public string License { get; set; }
        public string BusinessGroupId { get; set; }
        public string CreatedBy { get; set; }
        public int IRN { get; set; }
        public bool Status { get; set; }
    }
}