using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Supplier");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(e => e.SupplierCode)
                    .IsRequired();

            builder.Property(e => e.SupplierName)
                    .IsRequired();

            builder.HasOne(p => p.Phones).WithMany(sc => sc.Suppliers).HasForeignKey(sc => sc.PhoneId)
                    .IsRequired();

            builder.HasOne(s => s.Address).WithMany(sc => sc.Suppliers).HasForeignKey(sc => sc.AddressId)
                    .IsRequired();

            builder.HasOne(tp => tp.TypePerson).WithMany(sc => sc.Suppliers).HasForeignKey(sc => sc.TypePersonId)
                    .IsRequired();
        }
    }
}