﻿using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface IErrorService 
    {
        void GetTheConnectionString(string ConnectionString);
        void InsertBulk(IEnumerable<DocumentRejectedDTO> dto);
    }
}
