using eInvoicing.Service.AppService.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.Repository.Contract;
using eInvoicing.Service.Helper;
using DatingApp.API.Dtos;
using eInvoicing.DTO;
using eInvoicing.Service.Helper.Extension;

namespace eInvoicing.Service.AppService.Implementation
{
    public class InternalAuthService : IInternalAuthService
    {
        private readonly IAuthRepository repository;
        private readonly IUserRoleRepository _userRole;
        private readonly IPermissionRepository _permissionRepo;

        public InternalAuthService(IAuthRepository _repository, IUserRoleRepository userRole, IPermissionRepository permissionRepo)
        {
            this.repository = _repository;
            this._userRole = userRole;
            this._permissionRepo = permissionRepo;
        }
        public UserDTO Login(string UserName, string Password)
        {
            try
            {
                var _user = repository.Get(UserName);
                if (_user == null)
                    return null;
                if (!VerfiyPasswordHash(Password, _user.PasswordHash, _user.PasswordSalt))
                    return null;
                var response = _user.ToUserDTO();
                string Token = JwtManager.GenerateToken(response);
                response.Token = Token;
                if (!string.IsNullOrEmpty(Token))
                    return response;
                return null;
            }
            catch
            {
                return null;
            }
        }
        public bool Register(RegistrationModelDTO model)
        {
            try
            {
                var user = AutoMapperConfiguration.Mapper.Map<User>(model);
                byte[] PasswordHash, PasswordSalt;
                CreatePasswordHash(model.Password, out PasswordHash, out PasswordSalt);
                user.PasswordSalt = PasswordSalt;
                user.PasswordHash = PasswordHash;
                user.Id = Guid.NewGuid().ToString();
                var _user = repository.Add(user);
                AddRolesToUser(_user.Id, model.Roles);
                if (_user != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            }
        }
        private bool VerfiyPasswordHash(string Password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt))
            {
                var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
                for (int i = 0; i < ComputeHash.Length; i++)
                {
                    if (ComputeHash[i] != PasswordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool UserExists(string UserName)
        {
            try
            {
                if (repository.CheckIsExist(x => x.UserName == UserName))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            try
            {
                var users = repository.GetAllIncluding(null, null, "UserRoles.Role.RolePermissions.Permission").Select(i => i.ToUserDTO()).ToList();
                return users;
            }
            catch (Exception ex)
            {
                return new List<UserDTO>();
            }
        }
        public List<PermissionDTO> GetPermissions()
        {
            try
            {

                return AutoMapperConfiguration.Mapper.Map<List<PermissionDTO>>(_permissionRepo.GetAllIncluding().ToList());
            }
            catch (Exception ex)
            {
                return new List<PermissionDTO>();
            }
        }

        public EditModelDTO GetUser(string Id)
        {
            try
            {
                return AutoMapperConfiguration.Mapper.Map<EditModelDTO>(repository.GetAllIncluding(i => i.Id == Id, null, "UserRoles.Role")?.Select(i => i.ToEditModelDTO()).FirstOrDefault());
            }
            catch
            {
                return new EditModelDTO();
            }
        }

        public bool Edit(EditModelDTO obj)
        {
            try
            {
                var res = repository.GetById(obj.Id);
                res.Email = obj.Email;
                res.FirstName = obj.FirstName;
                res.LastName = obj.LastName;
                res.Title = obj.Title;
                res.PhoneNumber = obj.PhoneNumber;
                var _user = repository.Update(res);
                RemoveRolesFromUser(_user.Id);
                AddRolesToUser(_user.Id, obj.Roles);

                if (_user != null)
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

        public void AddRolesToUser(string UserId, List<string> Roles)
        {
            try
            {
                _userRole.AddRange(Roles.Select(i => new UserRole { UserId = UserId, RoleId = i, Id = Guid.NewGuid().ToString() }));
            }
            catch (Exception ex)
            {
            }
        }
        public void RemoveRolesFromUser(string UserId)
        {
            try
            {
                _userRole.DeleteRange(UserId);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
