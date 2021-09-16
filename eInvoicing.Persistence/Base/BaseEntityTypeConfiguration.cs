
using eInvoicing.DomainEntities.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace eInvoicing.Persistence.Base
{
    public class BaseEntityTypeConfiguration<TBase> : EntityTypeConfiguration<TBase> where TBase : BaseEntity
    {
        //public virtual void Configure(EntityTypeBuilder<TBase> builder)
        //{
        //    builder.HasKey(x => x.Id);
        //    builder.Property(x => x.CreatedBy).IsRequired();
        //    builder.Property(x => x.CreatedOn).IsRequired();
        //    builder.Property(x => x.LastModifiedBy);
        //    builder.Property(x => x.ModifiedOn);
        //    builder.Property(x => x.IsDeleted);
        //}
    }
}
