using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(e => e.CustomerName)
                    .IsRequired();

            builder.Property(e => e.CustomerLastName)
                    .IsRequired();

            builder.HasOne(p => p.Phones).WithMany(c => c.Customers).HasForeignKey(sc => sc.CustomerPhoneId)
                    .IsRequired();

            builder.Property(e => e.CustomerFax)
                    .IsRequired();

            builder.HasOne(a => a.Address).WithMany(c => c.Customers).HasForeignKey(sc => sc.AddressId)
                    .IsRequired();

            builder.Property(e => e.CreditLimit)
                    .IsRequired()
                    .HasColumnType("decimal(15,2)");

            builder.HasOne(tp => tp.TypePerson).WithMany(c => c.Customers).HasForeignKey(sc => sc.TypePersonId)
                    .IsRequired();
        builder.HasOne(u => u.User).WithOne(c => c.Customer).HasForeignKey<Customer>(c => c.IdUser)
                    .IsRequired();
        }
    }
}