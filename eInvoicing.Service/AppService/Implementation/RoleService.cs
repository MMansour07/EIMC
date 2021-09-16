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
        private readonly IRolePrivilegePermissionRepository _RolePrivilegePermission;
        private readonly IRolePrivilegeRepository _RolePrivilege;
        public RoleService(IRoleRepository _repository, IUserRoleRepository userRole, IRolePrivilegePermissionRepository RolePrivilegePermission, IRolePrivilegeRepository RolePrivilege)
        {
            this.repository = _repository;
            this._userRole = userRole;
            this._RolePrivilegePermission = RolePrivilegePermission;
            this._RolePrivilege = RolePrivilege;
        }
        public bool CreateRole(RoleDTO model)
        {
            try
            {
                var Role = new Role() {Id = Guid.NewGuid().ToString(), Name = model.Name, Description = model.Description};
                Role.Id = Guid.NewGuid().ToString();
                var _role = repository.Add(Role);
                var _rolePrivileges = AddPrivilegesToRole(_role.Id, model.Privileges);
                AddPermissionsToRole(_rolePrivileges.ToList(), model.Permissions);
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
                var Roles = repository.GetAllIncluding(null, null, "RolePrivileges.Privilege, RolePrivileges.RolePrivilegePermissions.Permission").Select(i => i.ToRoleViewModel());
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
              return repository.GetAllIncluding(i => i.Id == Id, null, "RolePrivileges.Privilege, RolePrivileges.RolePrivilegePermissions.Permission")?.Select(i => i.ToRoleDTO()).FirstOrDefault();
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
                RemovePriviligeFromRole(Role.Id);
                var _rolePrivileges = AddPrivilegesToRole(Role.Id, obj.Privileges);
                AddPermissionsToRole(_rolePrivileges.ToList(), obj.Permissions);
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

        public IEnumerable<RolePrivilege> AddPrivilegesToRole(string RoleId, List<string> Privileges)
        {
            try
            {
                var data = Privileges.Select(i => new RolePrivilege { RoleId = RoleId, PrivilegeId = i, Id = Guid.NewGuid().ToString() }).ToList();
                return _RolePrivilege.AddRange(data);
            }
            catch (Exception ex)
            {
                return new List<RolePrivilege>();
            }
        }
        public void AddPermissionsToRole(List<RolePrivilege> RolePrivilege, List<string> Permissions)
        {
            try
            {
                var input = new List<RolePrivilegePermission>() { };
                var response = Permissions.Select(o => new RolePrivilegePermission { Id = Guid.NewGuid().ToString(), PermissionId = o }).ToList();
                for (int i = 0; i < RolePrivilege.Count(); i++)
                {
                    for (int j = 0; j < Permissions.Count(); j++)
                    {
                        response[j].RolePrivilegeId = RolePrivilege[i].Id;
                    }
                }
                _RolePrivilegePermission.AddRange(response);
            }
            catch (Exception ex)
            {
            }
        }

        public void RemovePriviligeFromRole(string RoleId)
        {
            try
            {
                _RolePrivilege.DeleteRange(RoleId);
            }
            catch (Exception ex)
            {
            }
        }
        public void RemovePermissionFromRole(string RoleId)
        {
            try
            {
                _RolePrivilegePermission.DeleteRange(RoleId+RoleId);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
