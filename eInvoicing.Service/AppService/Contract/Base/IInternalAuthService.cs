using System.Collections.Generic;
using DatingApp.API.Dtos;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface IInternalAuthService
    {
        UserDTO Login(string UserName, string Password);
        bool Register(RegistrationModelDTO user);
        bool UserExists(string UserName);
        IEnumerable<UserDTO> GetUsers();
        EditModelDTO GetUser(string Id);
        bool Edit(EditModelDTO obj);
        bool Delete(string id);
        List<PermissionDTO> GetPermissions();
    }
}
