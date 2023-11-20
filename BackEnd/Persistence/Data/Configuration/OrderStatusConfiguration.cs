using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistence.Data.Configuration;
public class OrderStatusConfiguration : IEntityTypeConfiguration<StatusOrder>
{
    public void Configure(EntityTypeBuilder<StatusOrder> builder)
    {
        builder.ToTable("StatusOrder");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id);

        builder.Property(e => e.Description);
    }
}