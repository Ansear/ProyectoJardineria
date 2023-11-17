using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("office");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id);

        builder.HasOne(o => o.Address).WithMany(a => a.Offices).HasForeignKey(o => o.IdAddress);

        builder.HasOne(o => o.Phones).WithMany(p => p.Offices).HasForeignKey(o => o.IdPhone);
    }
}