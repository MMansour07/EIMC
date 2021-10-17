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
    public class AuthRepository : IdentityRepository<User>, IAuthRepository
    {
        private readonly IdentityContext _context;
        public AuthRepository(IdentityContext context) : base(context)
        {
            _context = context;
        }

        public new User Get(string UserName)
        {
            try
            {
                return _context.AppUsers.Include("UserRoles.Role.RolePermissions.Permission").AsNoTracking().FirstOrDefault(x => x.UserName == UserName);
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
