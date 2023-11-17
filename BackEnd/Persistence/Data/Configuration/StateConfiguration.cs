using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.ToTable("state");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id);

        builder.Property(s => s.Name).HasMaxLength(80);

        builder.HasOne(s => s.Countries).WithMany(c => c.States).HasForeignKey(s => s.IdCountry);
    }
}