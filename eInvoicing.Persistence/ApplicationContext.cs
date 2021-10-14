using System;
using System.Collections.Generic;
using System.Text;
using eInvoicing.DomainEntities.Entities;
using System.Data.Entity;
using eInvoicing.Persistence.Seeder;

namespace eInvoicing.Persistence
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(): base("name=eInvoicing_CS")
        {
            Database.SetInitializer(new DBContextSeeder());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<InvoiceLine>()
                .HasRequired<Document>(s => s.Document)
                .WithMany(g => g.InvoiceLines)
                .HasForeignKey<string>(s => s.DocumentId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Error>()
                .HasRequired<Document>(s => s.Document)
                .WithMany(g => g.Errors)
                .HasForeignKey<string>(s => s.DocumentId).WillCascadeOnDelete(true);

            modelBuilder.Entity<TaxableItem>()
                .HasRequired<InvoiceLine>(s => s.InvoiceLine)
                .WithMany(g => g.TaxableItems)
                .HasForeignKey<string>(s => s.InvoiceLineId).WillCascadeOnDelete(true);

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


            modelBuilder.Entity<Document>().Property(x => x.TotalAmount).HasPrecision(18, 5);
            modelBuilder.Entity<Document>().Property(x => x.ExtraDiscountAmount).HasPrecision(18, 5);
            modelBuilder.Entity<Document>().Property(x => x.TotalDiscountAmount).HasPrecision(18, 5);
            modelBuilder.Entity<Document>().Property(x => x.TotalSalesAmount).HasPrecision(18, 5);
            modelBuilder.Entity<Document>().Property(x => x.NetAmount).HasPrecision(18, 5);
            modelBuilder.Entity<Document>().Property(x => x.TotalItemsDiscountAmount).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.AmountEGP).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.AmountSold).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.DiscountAmount).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.ItemsDiscount).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.NetTotal).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.SalesTotal).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.Total).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.TotalTaxableFees).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.ValueDifference).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.Quantity).HasPrecision(18, 5);
            modelBuilder.Entity<TaxableItem>().Property(x => x.Amount).HasPrecision(18, 5);
        }
        public DbSet<Error> Errors { get; set; }
        public DbSet<User> AppUsers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
        public DbSet<TaxableItem> TaxableItems { get; set; }
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<TaxPayer> TaxPayers { get; set; }
    }
}
