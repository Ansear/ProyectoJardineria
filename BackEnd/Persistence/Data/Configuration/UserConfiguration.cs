using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        {
            builder.ToTable("user");

            builder.Property(p => p.Id)
            .IsRequired();

            builder.Property(p => p.Username)
            .HasColumnName("username")
            .HasColumnType("varchar")
            .HasMaxLength(50);


            builder.Property(p => p.Password)
           .HasColumnName("password")
           .HasColumnType("varchar")
           .HasMaxLength(255)
           .IsRequired();

            builder.Property(p => p.Email)
            .HasColumnName("email")
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();

            builder
           .HasMany(p => p.Rols)
           .WithMany(r => r.Users)
           .UsingEntity<UserRol>(

               j => j
               .HasOne(pt => pt.Rol)
               .WithMany(t => t.UsersRols)
               .HasForeignKey(ut => ut.RolId),

               j => j
               .HasOne(et => et.Usuario)
               .WithMany(et => et.UsersRols)
               .HasForeignKey(el => el.UsuarioId),

               j =>
               {
                   j.ToTable("userRol");
                   j.HasKey(t => new { t.UsuarioId, t.RolId });

               });

            builder.HasMany(p => p.RefreshTokens)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);
        }

    }
}