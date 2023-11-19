using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public class BossConfiguration : IEntityTypeConfiguration<Boss>
{
    public void Configure(EntityTypeBuilder<Boss> builder)
    {
        builder.ToTable("Boss");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id);

        builder.Property(e => e.Name)
             .IsRequired()
             .HasMaxLength(30);
    }
}
