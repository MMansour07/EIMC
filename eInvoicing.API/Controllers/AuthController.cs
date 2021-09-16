using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using AutoMapper;
using AutoMapper.Configuration;
using DatingApp.API.Dtos;
using eInvoicing.API.Filters;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
using eInvoicing.Service.Helper;
using Microsoft.IdentityModel.Tokens;
using ProductLicense;

namespace eInvoicing.API.Controllers
{
    
    public class AuthController : ApiController
    {
        private readonly IInternalAuthService _auth;
        private readonly IRoleService _roleService;
        public AuthController(IInternalAuthService auth, IRoleService roleService)
        {
            _auth = auth;
            _roleService = roleService;
        }
        [JwtAuthentication]
        [HttpPost]
        [Route("api/auth/Register")]
        public IHttpActionResult Register(RegistrationModelDTO model)
        {
            try
            {
                if (_auth.UserExists(model.UserName.ToLower()))
                {
                    return BadRequest();
                }
                if (!_auth.Register(model))
                    return InternalServerError();
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }
        [LicenseAuthorization]
        [HttpPost]
        [Route("api/auth/Login")]
        public IHttpActionResult Login(LoginModelDTO model)
        {
            try
            {
                var temp = _auth.Login(model.UserName.ToLower(), model.Password);
                return Ok(temp);
            }
            catch
            {
                return InternalServerError();
            }
            
        }
        [JwtAuthentication]
        [HttpGet]
        [Route("api/auth/getPrivileges")]
        public IHttpActionResult getPrivileges()
        {
            try
            {
                return Ok(new PrivilegeViewModel() { Privileges = _auth.GetPrivileges(), Permissions = _auth.GetPermissions()});;
            }
            catch
            {
                return InternalServerError();
            }

        }
        [JwtAuthentication]
        [HttpGet]
        [Route("api/auth/getUsers")]
        public IHttpActionResult GetUsers()
        {
            try
            {
                var users = _auth.GetUsers();
                return Ok(users.ToList());
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
        [JwtAuthentication]
        [HttpGet]
        [Route("api/auth/getRoles")]
        public IHttpActionResult GetRoles()
        {
            try
            {
                var Roles = _roleService.GetRoles().ToList();
                return Ok(Roles);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
        [JwtAuthentication]
        [HttpGet]
        [Route("api/auth/edit")]
        public IHttpActionResult edit(string Id)
        {
            try
            {
                var user = _auth.GetUser(Id);
                return Ok(new EditViewModel() { User = user, Roles = _roleService.GetRoles().ToList()});
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
        [JwtAuthentication]
        [HttpPut]
        [Route("api/auth/edit")]
        public IHttpActionResult edit(EditModelDTO obj)
        {
            try
            {
                if(_auth.Edit(obj))
                return Ok();
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
        [JwtAuthentication]
        [HttpDelete]
        [Route("api/auth/delete")]
        public IHttpActionResult Delete(string Id)
        {
            try
            {
                if (_auth.Delete(Id))
                    return Ok();
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
        [JwtAuthentication]
        [HttpPost]
        [Route("api/auth/createRole")]
        public IHttpActionResult CreateRole(RoleDTO model)
        {
            try
            {
                if (_roleService.RoleIsExist(model.Name.ToLower()))
                {
                    return BadRequest();
                }
                if (!_roleService.CreateRole(model))
                    return InternalServerError();
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }
        [JwtAuthentication]
        [HttpGet]
        [Route("api/auth/editRole")]
        public IHttpActionResult editRole(string Id)
        {
            try
            {
                var Role = _roleService.GetRole(Id);
                return Ok(new EditRoleModel() { Role = Role, Privileges = _auth.GetPrivileges(), Permissions = _auth.GetPermissions() });
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
        [JwtAuthentication]
        [HttpPut]
        [Route("api/auth/editRole")]
        public IHttpActionResult editRole(RoleDTO obj)
        {
            try
            {
                if (_roleService.Edit(obj))
                    return Ok();
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
        [JwtAuthentication]
        [HttpDelete]
        [Route("api/auth/deleteRole")]
        public IHttpActionResult DeleteRole(string Id)
        {
            try
            {
                if (_roleService.Delete(Id))
                    return Ok();
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
    }
}