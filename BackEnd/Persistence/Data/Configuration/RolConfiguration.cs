using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        // Aquí puedes configurar las propiedades de la entidad Marca
        // utilizando el objeto 'builder'.
        builder.ToTable("rol");
        builder.Property(p => p.Id)
                .IsRequired();
        builder.Property(p => p.Nombre)
        .HasColumnName("rolName")
        .HasColumnType("varchar")
        .HasMaxLength(50)
        .IsRequired();

    }
}
