using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuracion
{
    public class OrderCustomerEmployeeConfiguration : IEntityTypeConfiguration<OrderCustomerEmployee>
    {
        public void Configure(EntityTypeBuilder<OrderCustomerEmployee> builder)
        {
            builder.ToTable("OrderCustomerEmployee");

            builder.HasOne(p => p.Order)
                 .WithMany(p => p.OrderCustomerEmployees)
                 .HasForeignKey(p => p.IdOrder);

            builder.HasOne(p => p.Customer)
                 .WithMany(p => p.OrderCustomerEmployees)
                 .HasForeignKey(p => p.IdCustomer);

            builder.HasOne(p => p.Employee)
                 .WithMany(p => p.OrderCustomerEmployees)
                 .HasForeignKey(p => p.IdEmployee);

        }
    }
}