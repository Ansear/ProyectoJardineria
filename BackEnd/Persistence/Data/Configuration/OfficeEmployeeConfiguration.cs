using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration
{
    public class OfficeEmployeeConfiguration : IEntityTypeConfiguration<OfficeEmployee>
    {
        public void Configure(EntityTypeBuilder<OfficeEmployee> builder)
        {
            builder.ToTable("OfficeEmployee");

            builder.HasKey(e => new { e.IdOffice, e.IdEmployee });

            // builder.HasOne(p => p.Offices)
            //     .WithMany(p => p.OfficeEmployees)
            //     .HasForeignKey(p => p.IdOffice);

            // builder.HasOne(p => p.Employees)
            //     .WithMany(p => p.OfficeEmployees)
            //     .HasForeignKey(p => p.IdEmployee);
        }
    }
}