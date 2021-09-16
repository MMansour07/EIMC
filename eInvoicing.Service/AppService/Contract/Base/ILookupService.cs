using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface ILookupService
    {
        void InsertLicense(string encryptedLicenseValue, string createdBy);
        string GetEncryptedLicenseValue(string key);
    }
}
