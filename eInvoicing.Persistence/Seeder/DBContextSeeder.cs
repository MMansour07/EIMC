﻿using System;
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
                Id = "GET",
                Action = "GET",
                CreatedOn = DateTime.Now
            };
            Permission permission2 = new Permission()
            {
                Id = "POST",
                Action = "POST",
                CreatedOn = DateTime.Now
            };
            Permission permission3 = new Permission()
            {
                Id = "PUT",
                Action = "PUT",
                CreatedOn = DateTime.Now
            };
            Permission permission4 = new Permission()
            {
                Id = "DELETE",
                Action = "DELETE",
                CreatedOn = DateTime.Now
            };
            context.Permission.AddRange(new List<Permission>() { permission1, permission2, permission3, permission4 });
            //Pages
           Privilege privilege1 = new Privilege()
           {
               Id = Guid.NewGuid().ToString(),
               Controller = "Auth",
               CreatedOn = DateTime.Now
           };
            Privilege privilege2 = new Privilege()
            {
                Id = Guid.NewGuid().ToString(),
                Controller = "Master",
                CreatedOn = DateTime.Now
            };
            Privilege privilege3 = new Privilege()
            {
                Id = Guid.NewGuid().ToString(),
                Controller = "Document",
                CreatedOn = DateTime.Now
            };
            Privilege privilege4 = new Privilege()
            {
                Id = Guid.NewGuid().ToString(),
                Controller = "DocumentSubmission",
                CreatedOn = DateTime.Now
            };
            Privilege privilege5 = new Privilege()
            {
                Id = Guid.NewGuid().ToString(),
                Controller = "User",
                CreatedOn = DateTime.Now
            };
            Privilege privilege9 = new Privilege()
            {
                Id = Guid.NewGuid().ToString(),
                Controller = "Role",
                CreatedOn = DateTime.Now
            };
            Privilege privilege6 = new Privilege()
            {
                Id = Guid.NewGuid().ToString(),
                Controller = "lookup",
                CreatedOn = DateTime.Now
            };
            Privilege privilege7 = new Privilege()
            {
                Id = Guid.NewGuid().ToString(),
                Controller = "report",
                CreatedOn = DateTime.Now
            };
            Privilege privilege8 = new Privilege()
            {
                Id = Guid.NewGuid().ToString(),
                Controller = "AppSettings",
                CreatedOn = DateTime.Now
            };
            context.Privileges.AddRange(new List<Privilege>() { privilege1, privilege2, privilege3, privilege4, privilege5, privilege6, privilege7, privilege8, privilege9 });

            RolePrivilegePermission RolePrivilegePermission1 = new RolePrivilegePermission() {Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePrivilegePermission RolePrivilegePermission2 = new RolePrivilegePermission() {Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePrivilegePermission RolePrivilegePermission3 = new RolePrivilegePermission() {Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePrivilegePermission RolePrivilegePermission4 = new RolePrivilegePermission() {Id = Guid.NewGuid().ToString(), Permission = permission4 };
            List<RolePrivilegePermission> rolePrivilegePermission11 = new List<RolePrivilegePermission>();
            rolePrivilegePermission11.Add(RolePrivilegePermission1);
            rolePrivilegePermission11.Add(RolePrivilegePermission2);
            rolePrivilegePermission11.Add(RolePrivilegePermission3);
            rolePrivilegePermission11.Add(RolePrivilegePermission4);

            RolePrivilegePermission RolePrivilegePermission5 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePrivilegePermission RolePrivilegePermission6 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePrivilegePermission RolePrivilegePermission7 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePrivilegePermission RolePrivilegePermission8 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission4 };
            List<RolePrivilegePermission> rolePrivilegePermission22 = new List<RolePrivilegePermission>();
            rolePrivilegePermission22.Add(RolePrivilegePermission5);
            rolePrivilegePermission22.Add(RolePrivilegePermission6);
            rolePrivilegePermission22.Add(RolePrivilegePermission7);
            rolePrivilegePermission22.Add(RolePrivilegePermission8);

            RolePrivilegePermission RolePrivilegePermission9 = new RolePrivilegePermission()  { Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePrivilegePermission RolePrivilegePermission10 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePrivilegePermission RolePrivilegePermission11 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePrivilegePermission RolePrivilegePermission12 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission4 };
            List<RolePrivilegePermission> rolePrivilegePermission33 = new List<RolePrivilegePermission>();

            rolePrivilegePermission33.Add(RolePrivilegePermission9);
            rolePrivilegePermission33.Add(RolePrivilegePermission10);
            rolePrivilegePermission33.Add(RolePrivilegePermission11);
            rolePrivilegePermission33.Add(RolePrivilegePermission12);

            RolePrivilegePermission RolePrivilegePermission13 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePrivilegePermission RolePrivilegePermission14 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePrivilegePermission RolePrivilegePermission15 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePrivilegePermission RolePrivilegePermission16 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission4 };
            List<RolePrivilegePermission> rolePrivilegePermission44 = new List<RolePrivilegePermission>();

            rolePrivilegePermission44.Add(RolePrivilegePermission13);
            rolePrivilegePermission44.Add(RolePrivilegePermission14);
            rolePrivilegePermission44.Add(RolePrivilegePermission15);
            rolePrivilegePermission44.Add(RolePrivilegePermission16);

            RolePrivilegePermission RolePrivilegePermission17 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePrivilegePermission RolePrivilegePermission18 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePrivilegePermission RolePrivilegePermission19 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePrivilegePermission RolePrivilegePermission20 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission4 };
            List<RolePrivilegePermission> rolePrivilegePermission55 = new List<RolePrivilegePermission>();

            rolePrivilegePermission55.Add(RolePrivilegePermission17);
            rolePrivilegePermission55.Add(RolePrivilegePermission18);
            rolePrivilegePermission55.Add(RolePrivilegePermission19);
            rolePrivilegePermission55.Add(RolePrivilegePermission20);

            RolePrivilegePermission RolePrivilegePermission21 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePrivilegePermission RolePrivilegePermission22 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePrivilegePermission RolePrivilegePermission23 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePrivilegePermission RolePrivilegePermission24 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission4 };
            List<RolePrivilegePermission> rolePrivilegePermission66 = new List<RolePrivilegePermission>();

            rolePrivilegePermission66.Add(RolePrivilegePermission21);
            rolePrivilegePermission66.Add(RolePrivilegePermission22);
            rolePrivilegePermission66.Add(RolePrivilegePermission23);
            rolePrivilegePermission66.Add(RolePrivilegePermission24);

            RolePrivilegePermission RolePrivilegePermission25 = new RolePrivilegePermission() {Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePrivilegePermission RolePrivilegePermission26 = new RolePrivilegePermission() {Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePrivilegePermission RolePrivilegePermission27 = new RolePrivilegePermission() {Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePrivilegePermission RolePrivilegePermission28 = new RolePrivilegePermission() {Id = Guid.NewGuid().ToString(), Permission = permission4 };

            List<RolePrivilegePermission> rolePrivilegePermission77 = new List<RolePrivilegePermission>();

            rolePrivilegePermission77.Add(RolePrivilegePermission25);
            rolePrivilegePermission77.Add(RolePrivilegePermission26);
            rolePrivilegePermission77.Add(RolePrivilegePermission27);
            rolePrivilegePermission77.Add(RolePrivilegePermission28);

            RolePrivilegePermission RolePrivilegePermission29 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePrivilegePermission RolePrivilegePermission30 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePrivilegePermission RolePrivilegePermission31 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePrivilegePermission RolePrivilegePermission32 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission4 };
            List<RolePrivilegePermission> rolePrivilegePermission88 = new List<RolePrivilegePermission>();


            rolePrivilegePermission88.Add(RolePrivilegePermission29);
            rolePrivilegePermission88.Add(RolePrivilegePermission30);
            rolePrivilegePermission88.Add(RolePrivilegePermission31);
            rolePrivilegePermission88.Add(RolePrivilegePermission32);

            RolePrivilegePermission RolePrivilegePermission33 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission1 };
            RolePrivilegePermission RolePrivilegePermission34 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission2 };
            RolePrivilegePermission RolePrivilegePermission35 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission3 };
            RolePrivilegePermission RolePrivilegePermission36 = new RolePrivilegePermission() { Id = Guid.NewGuid().ToString(), Permission = permission4 };

            List<RolePrivilegePermission> rolePrivilegePermission99 = new List<RolePrivilegePermission>();

            rolePrivilegePermission99.Add(RolePrivilegePermission33);
            rolePrivilegePermission99.Add(RolePrivilegePermission34);
            rolePrivilegePermission99.Add(RolePrivilegePermission35);
            rolePrivilegePermission99.Add(RolePrivilegePermission36);


            RolePrivilege rolePrivilege1 = new RolePrivilege() { Id = Guid.NewGuid().ToString(), Privilege = privilege1, RolePrivilegePermissions = rolePrivilegePermission11 };
            RolePrivilege rolePrivilege2 = new RolePrivilege() { Id = Guid.NewGuid().ToString(), Privilege = privilege2, RolePrivilegePermissions = rolePrivilegePermission22 };
            RolePrivilege rolePrivilege3 = new RolePrivilege() { Id = Guid.NewGuid().ToString(), Privilege = privilege3, RolePrivilegePermissions = rolePrivilegePermission33 };
            RolePrivilege rolePrivilege4 = new RolePrivilege() { Id = Guid.NewGuid().ToString(), Privilege = privilege4, RolePrivilegePermissions = rolePrivilegePermission44 };
            RolePrivilege rolePrivilege5 = new RolePrivilege() { Id = Guid.NewGuid().ToString(), Privilege = privilege5, RolePrivilegePermissions = rolePrivilegePermission55 };
            RolePrivilege rolePrivilege6 = new RolePrivilege() { Id = Guid.NewGuid().ToString(), Privilege = privilege6, RolePrivilegePermissions = rolePrivilegePermission66 };
            RolePrivilege rolePrivilege7 = new RolePrivilege() { Id = Guid.NewGuid().ToString(), Privilege = privilege7, RolePrivilegePermissions = rolePrivilegePermission77 };
            RolePrivilege rolePrivilege8 = new RolePrivilege() { Id = Guid.NewGuid().ToString(), Privilege = privilege8, RolePrivilegePermissions = rolePrivilegePermission88 };
            RolePrivilege rolePrivilege9 = new RolePrivilege() { Id = Guid.NewGuid().ToString(), Privilege = privilege9, RolePrivilegePermissions = rolePrivilegePermission99 };
            List<RolePrivilege> rolePrivilege = new List<RolePrivilege>();

            rolePrivilege.Add(rolePrivilege1);
            rolePrivilege.Add(rolePrivilege2);
            rolePrivilege.Add(rolePrivilege3);
            rolePrivilege.Add(rolePrivilege4);
            rolePrivilege.Add(rolePrivilege5);
            rolePrivilege.Add(rolePrivilege6);
            rolePrivilege.Add(rolePrivilege7);
            rolePrivilege.Add(rolePrivilege8);
            rolePrivilege.Add(rolePrivilege9);
            Role Role1 = new Role()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SuperAdmin",
                Description = "The shop worker on the insertion team will be stationed at the insertion table. This is an 8-hour team shift (6:30am-4:00pm). The job requires inserting fixed and variable metal and plastic pieces with wood glue and a hammer into shelve units for closets. It is a continuous action and a vital role in our shop. This station collaborates with the production of our other customized materials.",
                RolePrivileges = rolePrivilege
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
            Lookup lookup = new Lookup()
            {
                Key = "License",
                Value = "IYjB+HUrjKmTFFeCIng5o2y4K874CJSkyx1MqpQThRoPt+1OEdm06K2sjNIVqQLeDUpXF2dziOB9ySC+wI/qT0z+cw+i249qT5QU0mf4lFsogEK40HFx3w+gM8QRXKPIc7SIF03o9hoi/BOu4jgSj5fKAC5FQDhwvUBqUItfUVo="
            };

            context.AppUsers.Add(user1);
            context.Lookups.Add(lookup);
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