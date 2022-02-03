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
                    RegistrationDate = obj.RegistrationDate,
                    BusinessGroupId = obj.BusinessGroupId
                };
                repository.Add(input);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string TokenByBusinessGroup(string BusinessGroup)
        {
            try
            {
                return repository.Get(x => x.Status == true && 
                x.BusinessGroup.GroupName == BusinessGroup, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault()?.token;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string TokenByBusinessGroupId(string BGID)
        {
            try
            {
                return repository.Get(x => x.Status == true &&
                x.BusinessGroupId == BGID, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault()?.token;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //public string GetClientId(string Environment)
        //{
        //    try
        //    {
        //        if (Environment.ToLower() == "Prod")
        //            return repository.Get(x => x.Status == true, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault()?.ProdClientId;
        //        else
        //            return repository.Get(x => x.Status == true, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault()?.PreProdClientId;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public string GetClientIdByBusinessGroup(string BusinessGroup)
        {
            try
            {
                return repository.Get(x => x.Status == true && 
                x.BusinessGroup.GroupName == BusinessGroup, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault()?.PreProdClientId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GetClientIdByBusinessGroupId(string BGID)
        {
            try
            {
                return repository.Get(x => x.Status == true &&
                x.BusinessGroupId == BGID, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault()?.PreProdClientId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public TaxpayerDTO getTaxpayerDetails(string BusinessGroupId)
        {
            try
            {
                var taxpayer = repository.Get(x => x.Status == true && x.BusinessGroupId == BusinessGroupId, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault();
                if (taxpayer != null)
                {
                    var output = new TaxpayerDTO()
                    {
                        TaxPayerNameEn = taxpayer.NameEn,
                        TaxPayerNameAr = taxpayer.NameAr,
                        IRN = taxpayer.IRN,
                        ProdClientId = taxpayer.ProdClientId,
                        PreProdClientId = taxpayer.PreProdClientId,
                        ProdClientSecret = taxpayer.ProdClientSecret,
                        PreProdClientSecret = taxpayer.PreProdClientSecret,
                        ClientSecretExpirationDate = taxpayer.ClientSecretExpDate,
                        CreatedBy = taxpayer.CreatedBy,
                        LicenseCreationDate = taxpayer.CreationDate,
                        LicenseExpirationDate = taxpayer.ExpirationDate,
                        ERPAr = taxpayer.ERPAr,
                        ERPName = taxpayer.ERPEn,
                        LicenseType = taxpayer.LicenseType,
                        RegistrationNumber = taxpayer.RegistrationNumber,
                        RegistrationDate = taxpayer.RegistrationDate,
                        PIN = taxpayer.BusinessGroup.Token
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

        public TaxpayerDTO getTaxpayerDetailsByBusinessGroupName(string BusinessGroup)
        {
            try
            {
                var taxpayer = repository.Get(x => x.Status == true && x.BusinessGroup.GroupName == BusinessGroup, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault();
                if (taxpayer != null)
                {
                    var output = new TaxpayerDTO()
                    {
                        TaxPayerNameEn = taxpayer.NameEn,
                        TaxPayerNameAr = taxpayer.NameAr,
                        IRN = taxpayer.IRN,
                        ProdClientId = taxpayer.ProdClientId,
                        PreProdClientId = taxpayer.PreProdClientId,
                        ProdClientSecret = taxpayer.ProdClientSecret,
                        PreProdClientSecret = taxpayer.PreProdClientSecret,
                        ClientSecretExpirationDate = taxpayer.ClientSecretExpDate,
                        CreatedBy = taxpayer.CreatedBy,
                        LicenseCreationDate = taxpayer.CreationDate,
                        LicenseExpirationDate = taxpayer.ExpirationDate,
                        ERPAr = taxpayer.ERPAr,
                        ERPName = taxpayer.ERPEn,
                        LicenseType = taxpayer.LicenseType,
                        RegistrationNumber = taxpayer.RegistrationNumber,
                        RegistrationDate = taxpayer.RegistrationDate,
                        PIN = taxpayer.BusinessGroup.Token
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

        public bool CheckIsDBSync(string BusinessGroup)
        {
            try
            {
                var taxpayer = repository.Get(x => x.Status == true && x.BusinessGroup.GroupName == BusinessGroup, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault();
                if (taxpayer != null)
                    return taxpayer.BusinessGroup.IsDBSync;
                else
                    return false;
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
                taxpayer.PreProdClientId = obj.PreProdClientId;
                taxpayer.PreProdClientSecret = obj.PreProdClientSecret;
                taxpayer.ProdClientId= obj.ProdClientId;
                taxpayer.ProdClientSecret = obj.ProdClientSecret;
                taxpayer.TokenPin = obj.PIN;
                repository.Update(taxpayer);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

