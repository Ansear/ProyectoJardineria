using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
    public DbSet<Boss> Bosses { get; set; }
    public DbSet<OfficeEmployee> OfficeEmployees { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}