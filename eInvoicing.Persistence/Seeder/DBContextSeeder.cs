using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.Persistence;

namespace eInvoicing.Persistence.Seeder
{
    public class DBContextSeeder : DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            Permission permission1 = new Permission()
            {
                Id = "Can Submit",
                Action = "submit",
                CreatedOn = DateTime.Now
            };
            Permission permission2 = new Permission()
            {
                Id = "Can Submit All",
                Action = "auto_submit",
                CreatedOn = DateTime.Now
            };
            Permission permission3 = new Permission()
            {
                Id = "Change Settings",
                Action = "update",
                CreatedOn = DateTime.Now
            };
            Permission permission4 = new Permission()
            {
                Id = "Create Role",
                Action = "createrole",
                CreatedOn = DateTime.Now
            };
            Permission permission26 = new Permission()
            {
                Id = "Create User",
                Action = "createuser",
                CreatedOn = DateTime.Now
            };
            Permission permission5 = new Permission()
            {
                Id = "Delete Role",
                Action = "deleterole",
                CreatedOn = DateTime.Now
            };
            Permission permission6 = new Permission()
            {
                Id = "Delete User",
                Action = "deleteuser",
                CreatedOn = DateTime.Now
            };
            Permission permission7 = new Permission()
            {
                Id = "Edit Role",
                Action = "editrole",
                CreatedOn = DateTime.Now
            };
            Permission permission8 = new Permission()
            {
                Id = "Edit User",
                Action = "edituser",
                CreatedOn = DateTime.Now
            };
            Permission permission9 = new Permission()
            {
                Id = "Upload License",
                Action = "token",
                CreatedOn = DateTime.Now
            };
            Permission permission10 = new Permission()
            {
                Id = "Upload Document From External Sheet",
                Action = "uploadfile",
                CreatedOn = DateTime.Now
            };
            Permission permission11 = new Permission()
            {
                Id = "View All Pending Documents",
                Action = "ajax_pending",
                CreatedOn = DateTime.Now
            };
            Permission permission12 = new Permission()
            {
                Id = "View All Pending Invoice Lines",
                Action = "items",
                CreatedOn = DateTime.Now
            };
            Permission permission13 = new Permission()
            {
                Id = "View All Roles",
                Action = "getroles",
                CreatedOn = DateTime.Now
            };
            Permission permission14 = new Permission()
            {
                Id = "View All Submitted Documents",
                Action = "ajax_submitted",
                CreatedOn = DateTime.Now
            };
            Permission permission15 = new Permission()
            {
                Id = "View All Submitted Invoice Lines",
                Action = "submitted_items",
                CreatedOn = DateTime.Now
            };
            Permission permission16 = new Permission()
            {
                Id = "View All Users",
                Action = "getusers",
                CreatedOn = DateTime.Now
            };
            Permission permission17 = new Permission()
            {
                Id = "View Dashboard",
                Action = "renderer",
                CreatedOn = DateTime.Now
            };
            Permission permission18 = new Permission()
            {
                Id = "View Document Statistics Report",
                Action = "ajaxgetsubmitteddocumentsstats",
                CreatedOn = DateTime.Now
            };
            Permission permission19 = new Permission()
            {
                Id = "View Monthly Bestseller Report",
                Action = "ajaxgetmonthlybestseller",
                CreatedOn = DateTime.Now
            };
            Permission permission20 = new Permission()
            {
                Id = "View Monthly Lowestseller Report",
                Action = "ajaxgetmonthlylowestseller",
                CreatedOn = DateTime.Now
            };
            Permission permission21 = new Permission()
            {
                Id = "View Pending Document Details",
                Action = "details",
                CreatedOn = DateTime.Now
            };
            Permission permission22 = new Permission()
            {
                Id = "View Pending Documents Count",
                Action = "draftcount",
                CreatedOn = DateTime.Now
            };
            Permission permission23 = new Permission()
            {
                Id = "View Submitted Document Details",
                Action = "raw",
                CreatedOn = DateTime.Now
            };
            Permission permission24 = new Permission()
            {
                Id = "View Submitted Documents Count",
                Action = "sentcount",
                CreatedOn = DateTime.Now
            };
            Permission permission25 = new Permission()
            {
                Id = "View Top Goods Report",
                Action = "ajaxtopgoodsusage",
                CreatedOn = DateTime.Now
            };
            Permission permission27 = new Permission()
            {
                Id = "View Taxpayer Details",
                Action = "taxpayer_details",
                CreatedOn = DateTime.Now
            };

            Permission permission28 = new Permission()
            {
                Id = "Get Taxpayer Details",
                Action = "ajaxtaxpayerdetails",
                CreatedOn = DateTime.Now
            };

            

            context.Permission.AddRange(new List<Permission>() { permission1, permission2, permission3, permission4 });


            RolePermission RolePrivilegePermission1 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePermission RolePrivilegePermission2 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePermission RolePrivilegePermission3 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePermission RolePrivilegePermission4 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission4 };
            RolePermission RolePrivilegePermission5 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission5 };
            RolePermission RolePrivilegePermission6 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission6 };
            RolePermission RolePrivilegePermission7 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission7 };
            RolePermission RolePrivilegePermission8 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission8 };
            RolePermission RolePrivilegePermission9 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission9 };
            RolePermission RolePrivilegePermission10 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission10 };
            RolePermission RolePrivilegePermission11 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission11 };
            RolePermission RolePrivilegePermission12 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission12 };
            RolePermission RolePrivilegePermission13 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission13 };
            RolePermission RolePrivilegePermission14 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission14 };
            RolePermission RolePrivilegePermission15 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission15 };
            RolePermission RolePrivilegePermission16 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission16 };
            RolePermission RolePrivilegePermission17 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission17 };
            RolePermission RolePrivilegePermission18 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission18 };
            RolePermission RolePrivilegePermission19 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission19 };
            RolePermission RolePrivilegePermission20 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission20 };
            RolePermission RolePrivilegePermission21 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission21 };
            RolePermission RolePrivilegePermission22 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission22 };
            RolePermission RolePrivilegePermission23 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission23 };
            RolePermission RolePrivilegePermission24 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission24 };
            RolePermission RolePrivilegePermission25 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission25 };
            RolePermission RolePrivilegePermission26 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission26 };
            RolePermission RolePrivilegePermission27 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission27 };
            RolePermission RolePrivilegePermission28 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission28 };



            List<RolePermission> rolePermissions = new List<RolePermission>();
            rolePermissions.Add(RolePrivilegePermission1);
            rolePermissions.Add(RolePrivilegePermission2);
            rolePermissions.Add(RolePrivilegePermission3);
            rolePermissions.Add(RolePrivilegePermission4);
            rolePermissions.Add(RolePrivilegePermission5);
            rolePermissions.Add(RolePrivilegePermission6);
            rolePermissions.Add(RolePrivilegePermission7);
            rolePermissions.Add(RolePrivilegePermission8);
            rolePermissions.Add(RolePrivilegePermission9);
            rolePermissions.Add(RolePrivilegePermission10);
            rolePermissions.Add(RolePrivilegePermission11);
            rolePermissions.Add(RolePrivilegePermission12);
            rolePermissions.Add(RolePrivilegePermission13);
            rolePermissions.Add(RolePrivilegePermission14);
            rolePermissions.Add(RolePrivilegePermission15);
            rolePermissions.Add(RolePrivilegePermission16);
            rolePermissions.Add(RolePrivilegePermission17);
            rolePermissions.Add(RolePrivilegePermission18);
            rolePermissions.Add(RolePrivilegePermission19);
            rolePermissions.Add(RolePrivilegePermission20);
            rolePermissions.Add(RolePrivilegePermission21);
            rolePermissions.Add(RolePrivilegePermission22);
            rolePermissions.Add(RolePrivilegePermission23);
            rolePermissions.Add(RolePrivilegePermission24);
            rolePermissions.Add(RolePrivilegePermission25);
            rolePermissions.Add(RolePrivilegePermission26);
            rolePermissions.Add(RolePrivilegePermission27);
            rolePermissions.Add(RolePrivilegePermission28);

            Role Role1 = new Role()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SuperAdmin",
                Description = "The job requires inserting fixed and variable metal and plastic pieces with wood glue and a hammer into shelve units for closets. It is a continuous action and a vital role in our shop. This station collaborates with the production of our other customized materials.",
                RolePermissions = rolePermissions
            };
            context.Roles.Add(Role1);
            UserRole userRole = new UserRole()
            {
                Id = Guid.NewGuid().ToString(),
                Role = Role1
            };
            List<UserRole> userRoles = new List<UserRole>();
            userRoles.Add(userRole);

            byte[] PasswordHash,PasswordSalt;
            CreatePasswordHash("P@ssw0rd", out PasswordHash, out PasswordSalt);

            User user1 = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "superadmin",
                LastName = "superadmin",
                Email = "superadmin@info.com",
                CreatedOn = DateTime.Now,
                PhoneNumber = "+1(13245)(7890)",
                Title = "IT Manager",
                UserName = "superadmin",
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt,
                UserRoles = userRoles
            };
            TaxPayer taxPayer = new TaxPayer()
            {
                Id = Guid.NewGuid().ToString(),
                ClientSecretExpDate = DateTime.Now,
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.Now,
                RegistrationDate = DateTime.Now,
                LicenseType = "Basic",
                Status = true,
                token = "IYjB+HUrjKmTFFeCIng5o2y4K874CJSkyx1MqpQThRoPt+1OEdm06K2sjNIVqQLeDUpXF2dziOB9ySC+wI/qT0z+cw+i249qT5QU0mf4lFsogEK40HFx3w+gM8QRXKPIc7SIF03o9hoi/BOu4jgSj5fKAC5FQDhwvUBqUItfUVo="
            };

            context.AppUsers.Add(user1);
            context.TaxPayers.Add(taxPayer);
            context.SaveChanges();
            base.Seed(context);
        }
        private void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            }
        }
    }
}