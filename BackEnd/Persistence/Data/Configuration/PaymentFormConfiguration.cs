using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class PaymentFormConfiguration : IEntityTypeConfiguration<PaymentForm>
    {
        public void Configure(EntityTypeBuilder<PaymentForm> builder)
        {
            builder.ToTable("PaymentForm");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(e => e.PaymentFormName)
                 .IsRequired()
                 .HasMaxLength(30);
            
        }
    }
}