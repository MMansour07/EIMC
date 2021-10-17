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
using eInvoicing.Service.Helper.Extension;
using eInvoicing.Service.Helper;

namespace eInvoicing.Service.AppService.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository repository;
        private readonly IUserRoleRepository _userRole;
        private readonly IRolePermissionRepository rolePermission;
        public RoleService(IRoleRepository _repository, IUserRoleRepository userRole, IRolePermissionRepository RolePermission)
        {
            this.repository = _repository;
            this._userRole = userRole;
            this.rolePermission = RolePermission;
        }
        public bool CreateRole(RoleDTO model)
        {
            try
            {
                var Role = new Role() {Id = Guid.NewGuid().ToString(), Name = model.Name, Description = model.Description, 
                    RolePermissions = model.Permissions.Select(x => new RolePermission() { Id = Guid.NewGuid().ToString(), PermissionId = x}).ToList()};
                Role.Id = Guid.NewGuid().ToString();
                var _role = repository.Add(Role);
                //var _rolePrivileges = AddPrivilegesToRole(_role.Id, model.Privileges);
                //AddPermissionsToRole(_rolePrivileges.ToList(), model.Permissions);
                if (_role != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public IEnumerable<RoleViewModel> GetRoles()
        {
            try
            {
                var Roles = repository.GetAllIncluding(x => x.Name.ToLower() != "superadmin", null, "RolePermissions.Permission").Select(i => i.ToRoleViewModel());
                return Roles;
            }
            catch (Exception ex)
            {
                return new List<RoleViewModel>();
            }
        }
        public RoleDTO GetRole(string Id)
        {
            try
            {
              return repository.GetAllIncluding(i => i.Id == Id, null, "RolePermissions.Permission")?.Select(i => i.ToRoleDTO()).FirstOrDefault();
            }
            catch
            {
                return new RoleDTO();
            }
        }
        public bool Edit(RoleDTO obj)
        {
            try
            {
                var res = repository.Get(obj.Id);
                res.Name = obj.Name;
                res.Description = obj.Description;
                res.ModifiedOn = DateTime.Now;
                var Role = repository.Update(res);
                RemovePermissionFromRole(Role.Id);
                AddPermissionsToRole(Role.Id, obj.Permissions);
                if (Role != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Delete(string id)
        {
            try
            {
                repository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        public bool RoleIsExist(string RoleName)
        {
            try
            {
                if (repository.CheckIsExist(x => x.Name == RoleName))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void AddPermissionsToRole(string RoleId, List<string> Permissions)
        {
            try
            {
                try
                {
                    var data = Permissions.Select(i => new RolePermission 
                    { RoleId = RoleId, PermissionId = i, Id = Guid.NewGuid().ToString() }).ToList();
                    rolePermission.AddRange(data);
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void RemovePermissionFromRole(string RoleId)
        {
            try
            {
                rolePermission.DeleteRange(RoleId);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
