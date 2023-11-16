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
            builder.ToTable("Csutomer");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(e => e.CustomerName)
                    .IsRequired();

            builder.Property(e => e.CustomerLastName)
                    .IsRequired();

            builder.Property(e => e.CustomerPhoneId)
                    .IsRequired();

            builder.Property(e => e.CustomerFax)
                    .IsRequired();

            builder.Property(e => e.AddressId)
                    .IsRequired();

            builder.Property(e => e.CreditLimit)
                    .IsRequired()
                    .HasColumnType("decimal(15,2)");

            builder.Property(e => e.TypePersonId)
                    .IsRequired();
        }
    }
}