using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuracion
{
    public class ProductSupplierConfiguration : IEntityTypeConfiguration<ProductSupplier>
    {
        public void Configure(EntityTypeBuilder<ProductSupplier> builder)
        {
            builder.ToTable("ProductSupplier");

            builder.HasKey(e => new { e.IdSupplier, e.IdProduct });

            builder.HasOne(e => e.Supplier)
            .WithMany(e => e.ProductSuppliers)
            .HasForeignKey(e => e.IdSupplier);

            builder.HasOne(e => e.Product)
            .WithMany(e => e.ProductSuppliers)
            .HasForeignKey(e => e.IdProduct);

        }
    }
}