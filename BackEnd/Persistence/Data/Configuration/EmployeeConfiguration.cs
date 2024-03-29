using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
                builder.ToTable("Employee");

                builder.HasKey(e => e.Id);
                builder.Property(e => e.Id);

                builder.Property(e => e.EmployeeName)
                        .IsRequired();

                builder.Property(e => e.EmployeeLastName)
                        .IsRequired();

                builder.Property(e => e.Extension)
                        .IsRequired();

                builder.Property(e => e.EmployeeEmail)
                        .IsRequired();

                builder.Property(e => e.EmployeePosition)
                        .IsRequired();

                builder.HasOne(p => p.Boss)
                    .WithMany()
                    .HasForeignKey(p => p.IdBoss).IsRequired(false);
        
        builder.HasOne(u => u.User).WithOne(c => c.Employee).HasForeignKey<Employee>(c => c.IdUser)
                    .IsRequired();
        }
}
