using System.Collections.Generic;
using DatingApp.API.Dtos;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface IBusinessGroupService
    {
        bool Create(BusinessGroupDTO user);
        IEnumerable<BusinessGroupDTO> GetBusinessGroups();
        BusinessGroupDTO GetBusinessGroup(string Id);
        bool Edit(BusinessGroupDTO obj);
        bool Delete(string id);
    }
}
