using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuracion
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(e => e.ProductName)
                 .HasMaxLength(30)
                 .IsRequired();

            builder.Property(e => e.ProductDimensions)
                 .HasMaxLength(25)
                 .IsRequired();

            builder.Property(e => e.ProductDescription)
                 .HasMaxLength(150)
                 .IsRequired();

            builder.Property(e => e.ProductSalesPrice)
                 .HasColumnType("int")
                 .IsRequired();

            builder.Property(e => e.InStockQuantity)
                   .HasColumnType("int")
                   .IsRequired();

            builder.Property(e => e.SupplierPrice)
                   .HasColumnType("int")
                   .IsRequired();

            builder.HasOne(p => p.Gamma)
                 .WithMany(p => p.Products)
                 .HasForeignKey(p => p.IdGamma);
            

        }
    }
}