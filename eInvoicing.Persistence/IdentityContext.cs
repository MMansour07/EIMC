using System;
using System.Collections.Generic;
using System.Text;
using eInvoicing.DomainEntities.Entities;
using System.Data.Entity;
using eInvoicing.Persistence.Seeder;

namespace eInvoicing.Persistence
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(): base("name=Identity_CS")
        {
            //Database.SetInitializer(new DBContextSeeder());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserRole>()
                .HasRequired<User>(s => s.User)
                .WithMany(g => g.UserRoles)
                .HasForeignKey<string>(s => s.UserId).WillCascadeOnDelete(true);

            modelBuilder.Entity<UserRole>()
                .HasRequired<Role>(s => s.Role)
                .WithMany(g => g.UserRoles)
                .HasForeignKey<string>(s => s.RoleId).WillCascadeOnDelete(true);

            modelBuilder.Entity<RolePermission>()
                .HasRequired<Role>(s => s.Role)
                .WithMany(g => g.RolePermissions)
                .HasForeignKey<string>(s => s.RoleId).WillCascadeOnDelete(true);

            modelBuilder.Entity<RolePermission>()
                .HasRequired<Permission>(s => s.Permission)
                .WithMany(g => g.RolePermission)
                .HasForeignKey<string>(s => s.PermissionId).WillCascadeOnDelete(true);

        }
        public DbSet<User> AppUsers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<TaxPayer> TaxPayers { get; set; }
        public DbSet<BusinessGroup> BusinessGroups { get; set; }
    }
}
