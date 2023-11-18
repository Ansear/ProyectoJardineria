using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuracion
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.HasOne(p => p.Product)
                 .WithMany(p => p.OrderDetails)
                 .HasForeignKey(p => p.ProductCode);

            builder.HasOne(p => p.Order)
                 .WithMany(p => p.OrderDetails)
                 .HasForeignKey(p => p.OrderCode);

            builder.Property(e => e.Quantity)
                 .HasColumnType("int")
                 .IsRequired();

            builder.Property(e => e.UnitPrice)
                 .HasColumnType("int")
                 .IsRequired();  

            builder.Property(e => e.LineNumber)
                 .IsRequired()
                 .HasMaxLength(35);
        }
    }
}