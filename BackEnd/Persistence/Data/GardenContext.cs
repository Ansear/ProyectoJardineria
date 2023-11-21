using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Data;
public class GardenContext : DbContext
{
    public GardenContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Address> Address { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Office> Offices { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<TypePerson> TypePersons { get; set; }
    public DbSet<OfficeEmployee> OfficeEmployees { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderCustomerEmployee> OrderCustomerEmployees { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductGamma> ProductGammas { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentForm> PaymentForms { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRol> UsersRols { get; set; }
    public DbSet<Rol> Rols { get; set; }
    public DbSet<StatusOrder> StatusOrders { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}