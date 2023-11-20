using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuracion
{
     public class OrderConfiguration : IEntityTypeConfiguration<Order>
     {
          public void Configure(EntityTypeBuilder<Order> builder)
          {
               builder.ToTable("Order");

               builder.HasKey(e => e.Id);
               builder.Property(e => e.Id);

               builder.Property(e => e.OrderDate)
                    .IsRequired()
                    .HasColumnType("DateTime");

               builder.Property(e => e.ExpectedDate)
                    .IsRequired()
                    .HasColumnType("DateTime");

               builder.Property(e => e.DeliveryDate)
               .IsRequired()
               .HasColumnType("DateTime");


               builder.Property(e => e.OrderComments)
                    .HasMaxLength(100);

               builder.HasOne(e => e.Payment)
                    .WithOne(f => f.Order)
                    .HasForeignKey<Order>(p => p.IdPayment);
               
               builder.HasOne(e => e.StatusOrder)
                    .WithMany(f => f.Orders)
                    .HasForeignKey(p => p.IdStatus);

               builder.Property(e => e.Total)
                    .HasColumnType("int")
                    .IsRequired();
          }
     }
}