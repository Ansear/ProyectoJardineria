using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public class AddressConfiguration : 
IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("address");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id);

        builder.Property(a => a.TypeRoad);

        builder.Property(a => a.MainNumber);

        builder.Property(a => a.MainLetter);

        builder.Property(a => a.Bis);

        builder.Property(a => a.SecondaryLetter);

        builder.Property(a => a.CardinalPrimary);

        builder.Property(a => a.SecondNumber);

        builder.Property(a => a.CardinalSecondary);

        builder.Property(a => a.Complement);

        builder.Property(a => a.ZipCode);

        builder.HasOne(a => a.Cities).WithMany(c => c.Address).HasForeignKey(a => a.IdCity);
    }
}