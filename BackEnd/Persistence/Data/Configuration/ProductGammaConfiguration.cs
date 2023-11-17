using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuracion
{
    public class ProductGammaConfiguration : IEntityTypeConfiguration<ProductGamma>
    {
        public void Configure(EntityTypeBuilder<ProductGamma> builder)
        {

            builder.ToTable("ProductGamma");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(e => e.TextDescription)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.HtmlDescription)
                 .IsRequired()
                 .HasMaxLength(150);

            builder.Property(e => e.Image)
                 .IsRequired()
                 .HasMaxLength(50);

        }
    }
}