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

namespace eInvoicing.Service.AppService.Implementation
{
    public class LookupService :  ILookupService
    {
        private readonly ILookupRepository repository;
        public LookupService(ILookupRepository _repository)
        {
            this.repository = _repository;
        }
        public void InsertLicense(string encryptedLicenseValue, string createdBy)
        {
            try
            {
                var obj = new Lookup() { Key = "License", Value = encryptedLicenseValue, CreatedOn = DateTime.Now, CreatedBy = createdBy};
                repository.Add(obj);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GetEncryptedLicenseValue(string key)
        {
            try
            {
                return repository.Get(x => x.Key.ToLower() == key, m => m.OrderByDescending(x => x.CreatedOn)).FirstOrDefault().Value;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

