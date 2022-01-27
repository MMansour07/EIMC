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
using eInvoicing.DTO.Base;

namespace eInvoicing.Service.AppService.Implementation
{
    public class ErrorService : IErrorService
    {

        private readonly IErrorReposistory _repository;
        public ErrorService(IErrorReposistory repository)
        {
            _repository = repository;
        }
        public void GetTheConnectionString(string ConnectionString)
        {
            this._repository.GetTheConnectionString(ConnectionString);
        }
        public void InsertBulk(IEnumerable<DocumentRejectedDTO> dto)
        {
            try
            {
                var res = dto.Select(i => i.ToErrorDto());
                var entities = res.Select(x => x.details).Select(e => e.ToEntityList<Error>());
                foreach (var item in entities)
                {
                    _repository.AddRange(item);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
