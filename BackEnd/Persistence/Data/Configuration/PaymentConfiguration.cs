using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuracion
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Aquí puedes configurar las propiedades de la entidad
            // utilizando el objeto builder
            builder.ToTable("Payment");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(e => e.PaymentDate)
                .IsRequired()
                .HasColumnType("DateTime");

            builder.Property(e => e.Total)
                 .IsRequired()
                 .HasColumnType("int");

            builder.HasOne(p => p.PaymentForm)
                 .WithMany(p => p.Payments)
                 .HasForeignKey(p => p.IdFormPay);

            builder.HasOne(p => p.Order)
                 .WithOne(p => p.Payments)
                 .HasForeignKey<Payment>(p => p.IdOrder);
        }
    }
}