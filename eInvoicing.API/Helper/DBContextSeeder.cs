using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using eInvoicing.DomainEntities.Entities;
using eInvoicing.Persistence;

namespace eInvoicing.API.Helper
{
    public class DBContextSeeder : DropCreateDatabaseIfModelChanges<IdentityContext>
    {
        protected override void Seed(IdentityContext context)
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
            Permission permission29 = new Permission()
            {
                Id = "Create New Document",
                Action = "new_document",
                CreatedOn = DateTime.Now
            };
            Permission permission30 = new Permission()
            {
                Id = "View New Document",
                Action = "ajax_new_document",
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
            RolePermission RolePrivilegePermission29 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission29 };
            RolePermission RolePrivilegePermission30 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission30 };

            RolePermission RolePrivilegePermission31 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePermission RolePrivilegePermission32 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePermission RolePrivilegePermission33 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePermission RolePrivilegePermission34 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission4 };
            RolePermission RolePrivilegePermission35 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission5 };
            RolePermission RolePrivilegePermission36 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission6 };
            RolePermission RolePrivilegePermission37 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission7 };
            RolePermission RolePrivilegePermission38 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission8 };
            RolePermission RolePrivilegePermission39 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission9 };
            RolePermission RolePrivilegePermission40 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission10 };
            RolePermission RolePrivilegePermission41 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission11 };
            RolePermission RolePrivilegePermission42 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission12 };
            RolePermission RolePrivilegePermission43 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission13 };
            RolePermission RolePrivilegePermission44 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission14 };
            RolePermission RolePrivilegePermission45 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission15 };
            RolePermission RolePrivilegePermission46 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission16 };
            RolePermission RolePrivilegePermission47 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission17 };
            RolePermission RolePrivilegePermission48 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission18 };
            RolePermission RolePrivilegePermission49 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission19 };
            RolePermission RolePrivilegePermission50 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission20 };
            RolePermission RolePrivilegePermission51 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission21 };
            RolePermission RolePrivilegePermission52 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission22 };
            RolePermission RolePrivilegePermission53 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission23 };
            RolePermission RolePrivilegePermission54 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission24 };
            RolePermission RolePrivilegePermission55 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission25 };
            RolePermission RolePrivilegePermission56 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission26 };
            RolePermission RolePrivilegePermission57 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission27 };
            RolePermission RolePrivilegePermission58 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission28 };
            RolePermission RolePrivilegePermission59 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission29 };
            RolePermission RolePrivilegePermission60 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission30 };

            RolePermission RolePrivilegePermission61 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePermission RolePrivilegePermission62 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePermission RolePrivilegePermission63 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePermission RolePrivilegePermission64 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission4 };
            RolePermission RolePrivilegePermission65 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission5 };
            RolePermission RolePrivilegePermission66 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission6 };
            RolePermission RolePrivilegePermission67 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission7 };
            RolePermission RolePrivilegePermission68 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission8 };
            RolePermission RolePrivilegePermission69 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission9 };
            RolePermission RolePrivilegePermission70 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission10 };
            RolePermission RolePrivilegePermission71 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission11 };
            RolePermission RolePrivilegePermission72 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission12 };
            RolePermission RolePrivilegePermission73 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission13 };
            RolePermission RolePrivilegePermission74 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission14 };
            RolePermission RolePrivilegePermission75 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission15 };
            RolePermission RolePrivilegePermission76 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission16 };
            RolePermission RolePrivilegePermission77 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission17 };
            RolePermission RolePrivilegePermission78 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission18 };
            RolePermission RolePrivilegePermission79 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission19 };
            RolePermission RolePrivilegePermission80 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission20 };
            RolePermission RolePrivilegePermission81 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission21 };
            RolePermission RolePrivilegePermission82 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission22 };
            RolePermission RolePrivilegePermission83 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission23 };
            RolePermission RolePrivilegePermission84 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission24 };
            RolePermission RolePrivilegePermission85 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission25 };
            RolePermission RolePrivilegePermission86 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission26 };
            RolePermission RolePrivilegePermission87 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission27 };
            RolePermission RolePrivilegePermission88 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission28 };
            RolePermission RolePrivilegePermission89 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission29 };
            RolePermission RolePrivilegePermission90 = new RolePermission() { Id = Guid.NewGuid().ToString(), Permission = permission30 };



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
            rolePermissions.Add(RolePrivilegePermission29);
            rolePermissions.Add(RolePrivilegePermission30);


            List<RolePermission> rolePermissions1 = new List<RolePermission>();
            rolePermissions1.Add(RolePrivilegePermission31);
            rolePermissions1.Add(RolePrivilegePermission32);
            rolePermissions1.Add(RolePrivilegePermission33);
            rolePermissions1.Add(RolePrivilegePermission34);
            rolePermissions1.Add(RolePrivilegePermission35);
            rolePermissions1.Add(RolePrivilegePermission36);
            rolePermissions1.Add(RolePrivilegePermission37);
            rolePermissions1.Add(RolePrivilegePermission38);
            rolePermissions1.Add(RolePrivilegePermission39);
            rolePermissions1.Add(RolePrivilegePermission40);
            rolePermissions1.Add(RolePrivilegePermission41);
            rolePermissions1.Add(RolePrivilegePermission42);
            rolePermissions1.Add(RolePrivilegePermission43);
            rolePermissions1.Add(RolePrivilegePermission44);
            rolePermissions1.Add(RolePrivilegePermission45);
            rolePermissions1.Add(RolePrivilegePermission46);
            rolePermissions1.Add(RolePrivilegePermission47);
            rolePermissions1.Add(RolePrivilegePermission48);
            rolePermissions1.Add(RolePrivilegePermission49);
            rolePermissions1.Add(RolePrivilegePermission50);
            rolePermissions1.Add(RolePrivilegePermission51);
            rolePermissions1.Add(RolePrivilegePermission52);
            rolePermissions1.Add(RolePrivilegePermission53);
            rolePermissions1.Add(RolePrivilegePermission54);
            rolePermissions1.Add(RolePrivilegePermission55);
            rolePermissions1.Add(RolePrivilegePermission56);
            rolePermissions1.Add(RolePrivilegePermission57);
            rolePermissions1.Add(RolePrivilegePermission58);
            rolePermissions1.Add(RolePrivilegePermission59);
            rolePermissions1.Add(RolePrivilegePermission60);


            List<RolePermission> rolePermissions2 = new List<RolePermission>();
            rolePermissions2.Add(RolePrivilegePermission61);
            rolePermissions2.Add(RolePrivilegePermission62);
            rolePermissions2.Add(RolePrivilegePermission63);
            rolePermissions2.Add(RolePrivilegePermission64);
            rolePermissions2.Add(RolePrivilegePermission65);
            rolePermissions2.Add(RolePrivilegePermission66);
            rolePermissions2.Add(RolePrivilegePermission67);
            rolePermissions2.Add(RolePrivilegePermission68);
            rolePermissions2.Add(RolePrivilegePermission69);
            rolePermissions2.Add(RolePrivilegePermission70);
            rolePermissions2.Add(RolePrivilegePermission71);
            rolePermissions2.Add(RolePrivilegePermission72);
            rolePermissions2.Add(RolePrivilegePermission73);
            rolePermissions2.Add(RolePrivilegePermission74);
            rolePermissions2.Add(RolePrivilegePermission75);
            rolePermissions2.Add(RolePrivilegePermission76);
            rolePermissions2.Add(RolePrivilegePermission77);
            rolePermissions2.Add(RolePrivilegePermission78);
            rolePermissions2.Add(RolePrivilegePermission79);
            rolePermissions2.Add(RolePrivilegePermission80);
            rolePermissions2.Add(RolePrivilegePermission81);
            rolePermissions2.Add(RolePrivilegePermission82);
            rolePermissions2.Add(RolePrivilegePermission83);
            rolePermissions2.Add(RolePrivilegePermission84);
            rolePermissions2.Add(RolePrivilegePermission85);
            rolePermissions2.Add(RolePrivilegePermission86);
            rolePermissions2.Add(RolePrivilegePermission87);
            rolePermissions2.Add(RolePrivilegePermission88);
            rolePermissions2.Add(RolePrivilegePermission89);
            rolePermissions2.Add(RolePrivilegePermission90);

            Role Role1 = new Role()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SuperAdmin",
                Description = "The job requires inserting fixed and variable metal and plastic pieces with wood glue and a hammer into shelve units for closets. It is a continuous action and a vital role in our shop. This station collaborates with the production of our other customized materials.",
                RolePermissions = rolePermissions
            };
            Role Role2 = new Role()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Administrator",
                Description = "IT administrators oversee organizations' computer systems and manage IT teams. They maintain information systems and networks, upgrade and install new hardware and software, and perform troubleshooting. They also back up data and manage network security..",
                RolePermissions = rolePermissions1
            };
            Role Role3 = new Role()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Business User",
                Description = "The standard user role suits business owners or admin staff responsible for managing day-to-day tasks, giving them almost full access to the Xero organisation.",
                RolePermissions = rolePermissions2
            };
            context.Roles.Add(Role1);
            context.Roles.Add(Role2);
            context.Roles.Add(Role3);

            UserRole userRole = new UserRole()
            {
                Id = Guid.NewGuid().ToString(),
                Role = Role1
            };
            List<UserRole> userRoles = new List<UserRole>();
            userRoles.Add(userRole);

            BusinessGroup BusinessGroup = new BusinessGroup()
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = "Product Owner",
                BusinessType = "None",
                IsDBSync = true,
                CreatedOn = DateTime.Now,
                SyncType = "DB",
                Token = "12345678",
                USB_SerialNumber = "12345678"
            };

            byte[] PasswordHash, PasswordSalt;
            CreatePasswordHash("P@ssw0rd", out PasswordHash, out PasswordSalt);

            User user1 = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Mohamed",
                LastName = "Mansour",
                Email = "superadmin@eta-dt.com",
                CreatedOn = DateTime.Now,
                PhoneNumber = "02447098701",
                Title = "System Administrator",
                UserName = "superadmin",
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt,
                UserRoles = userRoles,
                BusinessGroup = BusinessGroup
            };

            //TaxPayer taxPayer = new TaxPayer()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    ClientSecretExpDate = DateTime.Now,
            //    CreationDate = DateTime.Now,
            //    ExpirationDate = DateTime.Now.AddDays(7),
            //    RegistrationDate = DateTime.Now,
            //    LicenseType = "Basic",
            //    Status = true,
            //    PreProdClientId = "5e287ddc-d504-42e5-977d-ffc609237d64",
            //    token = "IYjB+HUrjKmTFFeCIng5o8QddjV0rGfwCuUK34Zhzg3v2iLmUJcge+vDfoRUOdYigZLx/dNNMVMEBf8uJPe5/GfMbp4ehCwvGxnMVl78e7Mi5FixZc7+9R0W//2ar2U9S7wbRRWN7e5jxc9KsV4IzYJMQ3Jy6PvuB1tNG2qNqtYo0kQTma1RUkNFriZBMSRXcoc6CpnNNQe1pkR6Sk2hu+GjU3i8yHFRw1/YTa2/iSeYk3J2kMcoTXsQyZe7Gq+Hz4c1Ku6H6Psb+uU63JnUqgdFldngbHkyt6lDalkvFXqRLSy3iYLablp5WKKJ8Ok+9YHXrjaqiPJDP1Q+cb56WpFvKdtOFy/TlO5SWzYPKl/Gx1yUQ3fzCAfERgPn8/7x/BEcNSMu7r0IKO7MWaoesQymwNj6pwRVG04plFnZGwMHj80lzslmxAWyxauMKNVvu/UI5Gi3tNi0o4V/3zTW7WOJ19rUq9KWyTXFV0QWs+5PE/cWre73OyNrjms3mtQB5LVJUIg3pWS0OcxfIaAz3Vcl7VePlAkdKdPjxkPCf9saHb46loFTnqPldLFaZEUY"
            //};

            context.AppUsers.Add(user1);
            //context.TaxPayers.Add(taxPayer);
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