using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface IRoleService
    {
        IEnumerable<RoleViewModel> GetRoles();
        bool CreateRole(RoleDTO model);
        RoleDTO GetRole(string Id);
        bool Edit(RoleDTO obj);
        bool Delete(string id);
        bool RoleIsExist(string RoleName);
    }
}
