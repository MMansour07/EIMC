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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<InvoiceLine>()
                .HasRequired<Document>(s => s.Document)
                .WithMany(g => g.InvoiceLines)
                .HasForeignKey<string>(s => s.DocumentId).WillCascadeOnDelete(true);

            //modelBuilder.Entity<ValidationStep>()
            //    .HasRequired<Document>(s => s.Document)
            //    .WithMany(g => g.ValidationSteps)
            //    .HasForeignKey<string>(s => s.DocumentId).WillCascadeOnDelete(true);

            //modelBuilder.Entity<StepError>()
            //    .HasRequired<ValidationStep>(s => s.ValidationStep)
            //    .WithMany(g => g.StepErrors)
            //    .HasForeignKey<string>(s => s.ValidationStepId).WillCascadeOnDelete(true);

            //modelBuilder.Entity<StepError>()
            //    .HasMany(x => x.InnerError)
            //    .WithRequired(x => x._StepError)
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Error>()
            //    .HasRequired<Document>(s => s.Document)
            //    .WithMany(g => g.Errors)
            //    .HasForeignKey<string>(s => s.DocumentId).WillCascadeOnDelete(true);

            modelBuilder.Entity<TaxableItem>()
                .HasRequired<InvoiceLine>(s => s.InvoiceLine)
                .WithMany(g => g.TaxableItems)
                .HasForeignKey<string>(s => s.InvoiceLineId).WillCascadeOnDelete(true);

            modelBuilder.Entity<Document>().Property(x => x.TotalAmount).HasPrecision(18, 5);
            modelBuilder.Entity<Document>().Property(x => x.ExtraDiscountAmount).HasPrecision(18, 5);
            modelBuilder.Entity<Document>().Property(x => x.TotalDiscountAmount).HasPrecision(18, 5);
            modelBuilder.Entity<Document>().Property(x => x.TotalSalesAmount).HasPrecision(18, 5);
            modelBuilder.Entity<Document>().Property(x => x.NetAmount).HasPrecision(18, 5);
            modelBuilder.Entity<Document>().Property(x => x.TotalItemsDiscountAmount).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.AmountEGP).HasPrecision(18, 5);
            modelBuilder.Entity<InvoiceLine>().Property(x => x.CurrencyExchangeRate).HasPrecision(18, 5);
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
        public DbSet<Document> Documents { get; set; }
        public DbSet<ValidationStep> ValidationSteps { get; set; }
        public DbSet<StepError> StepErrors { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
        public DbSet<TaxableItem> TaxableItems { get; set; }
    }
}
