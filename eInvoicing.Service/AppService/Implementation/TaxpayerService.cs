using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.AppService.Implementation.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using eInvoicing.Repository.Contract.Base;
using eInvoicing.DomainEntities.Entities;
using Ninject;
using eInvoicing.Repository.Contract;
using System.Reflection;
using eInvoicing.Repository.Implementation;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using eInvoicing.Service.Helper.Extension;
using System.Net;
using System.IO;
using System.Web;
using System.Globalization;
using Microsoft.Practices.ObjectBuilder2;
using System.Data.Entity.SqlServer;
using System.Linq.Dynamic;
using eInvoicing.Service.Helper;

namespace eInvoicing.Service.AppService.Implementation
{
    public class TaxpayerService : ITaxpayerService
    {
        private readonly ITaxpayerRepository repository;
        public TaxpayerService(ITaxpayerRepository _repository)
        {
            this.repository = _repository;
        }
        public void add(TaxpayerDTO obj)
        {
            try
            {
                var input = new TaxPayer()
                {
                    Id = Guid.NewGuid().ToString(),
                    NameEn = obj.TaxPayerNameEn,
                    NameAr = obj.TaxPayerNameAr,
                    IRN = obj.IRN,
                    ClientId = obj.ClientId,
                    ClientSecret = obj.ClientSecret,
                    ClientSecretExpDate = obj.ClientSecretExpirationDate,
                    CreatedBy = obj.CreatedBy,
                    CreatedOn = DateTime.Now,
                    CreationDate = obj.LicenseCreationDate,
                    ExpirationDate = obj.LicenseExpirationDate,
                    ERPAr = obj.ERPAr,
                    ERPEn = obj.ERPName,
                    LicenseType = obj.LicenseType,
                    RegistrationNumber = obj.RegistrationNumber,
                    IsDeleted = false,
                    Status = true,
                    token = obj.License,
                    RegistrationDate = obj.RegistrationDate
                };
                repository.Add(input);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string token()
        {
            try
            {
                return repository.Get(x => x.Status == true, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault()?.token;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GetClientId()
        {
            try
            {
                return repository.Get(x => x.Status == true, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault()?.ClientId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public TaxpayerDTO getTaxpayerDetails()
        {
            try
            {
                var taxpayer = repository.Get(x => x.Status == true, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault();
                if (taxpayer != null)
                {
                    var output = new TaxpayerDTO()
                    {
                        TaxPayerNameEn = taxpayer.NameEn,
                        TaxPayerNameAr = taxpayer.NameAr,
                        IRN = taxpayer.IRN,
                        ClientId = taxpayer.ClientId,
                        ClientSecret = taxpayer.ClientSecret,
                        ClientSecretExpirationDate = taxpayer.ClientSecretExpDate,
                        CreatedBy = taxpayer.CreatedBy,
                        LicenseCreationDate = taxpayer.CreationDate,
                        LicenseExpirationDate = taxpayer.ExpirationDate,
                        ERPAr = taxpayer.ERPAr,
                        ERPName = taxpayer.ERPEn,
                        LicenseType = taxpayer.LicenseType,
                        RegistrationNumber = taxpayer.RegistrationNumber,
                        RegistrationDate = taxpayer.RegistrationDate
                    };
                    return output;
                }
                return new TaxpayerDTO();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void updateTaxPayer(TaxpayerDTO obj)
        {
            try
            {
                var taxpayer = repository.GetLastRecord();
                taxpayer.ClientId = obj.ClientId;
                taxpayer.ClientSecret = obj.ClientSecret;
                repository.Update(taxpayer);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

