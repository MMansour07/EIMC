using System;
using System.Linq;
using System.Web.Http;
using DatingApp.API.Dtos;
using eInvoicing.API.Filters;
using eInvoicing.DTO;
using eInvoicing.Service.AppService.Contract.Base;
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
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //[LicenseAuthorization]
        [AllowAnonymous]
        [HttpPost]
        [Route("api/auth/Login")]
        public IHttpActionResult Login(LoginModelDTO model)
        {
            try
            {
                var temp = _auth.Login(model.UserName.ToLower(), model.Password);
                return Ok(temp);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("api/auth/changepassword")]
        public IHttpActionResult ChangePassword(ChangePasswordDTO model)
        {
            try
            {
                if(!_auth.ChangePassword(model))
                    return InternalServerError();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/auth/getPerimssions")]
        public IHttpActionResult getPrivileges()
        {
            try
            {
                return Ok(new PrivilegeViewModel() {Permissions = _auth.GetPermissions()});;
            } 
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        
        [JwtAuthentication]
        [HttpGet]
        [Route("api/auth/getUsers")]
        public IHttpActionResult GetUsers(string BusinessGroupId, string LoggedinUserName)
        {
            try
            {
                var users = _auth.GetUsers(BusinessGroupId, LoggedinUserName);
                return Ok(users.ToList());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
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
                return InternalServerError(ex);
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
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("api/auth/edit")]
        public IHttpActionResult edit(EditModelDTO obj)
        {
            try
            {
                if(_auth.Edit(obj))
                return Ok();

                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [JwtAuthentication]
        [HttpGet]
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
                return InternalServerError(ex);
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
                return Ok(new EditRoleModel() { Role = Role, Permissions = _auth.GetPermissions() });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        [JwtAuthentication]
        [HttpPost]
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
                return InternalServerError(ex);
            }
        }
        
        [JwtAuthentication]
        [HttpGet]
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
                return InternalServerError(ex);
            }
        }
    }
}