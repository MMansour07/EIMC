using eInvoicing.DomainEntities.Entities;
using eInvoicing.Persistence;
using eInvoicing.Repository.Contract;
using eInvoicing.Repository.Implementation.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Repository.Implementation
{
    public class AuthRepository : Repository<User>, IAuthRepository
    {
        private readonly ApplicationContext _context;
        public AuthRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public new User Get(string UserName)
        {
            try
            {
                return _context.AppUsers.Include("UserRoles.Role.RolePrivileges.Privilege").Include("UserRoles.Role.RolePrivileges.RolePrivilegePermissions.Permission").AsNoTracking().FirstOrDefault(x => x.UserName == UserName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public User GetById(string Id)
        {
            try
            {
                return _context.AppUsers.Include("UserRoles.Role").AsNoTracking().FirstOrDefault(x => x.Id == Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
