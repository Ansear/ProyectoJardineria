using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuracion
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payment");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(e => e.PaymentDate)
                .IsRequired()
                .HasColumnType("DateTime");

            builder.HasOne(p => p.PaymentForm)
                 .WithMany(p => p.Payments)
                 .HasForeignKey(p => p.IdFormPay);
        }
    }
}