using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface IAuthService
    {
        AuthDTO token(string URL, string grant_type, string client_id, string client_secret, string scope);
    }
}
