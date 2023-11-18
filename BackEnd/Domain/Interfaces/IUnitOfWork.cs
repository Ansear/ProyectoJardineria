using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IBoss Bosses { get; }
        ICustomer Customers { get; }
        IEmployee Employees { get; }
        IOfficeEmployee OfficeEmployees { get; }
        ISupplier Suppliers { get; }
        ITypePerson TypePersons { get; }
        IAddress Address { get; }
        ICountry Countries { get; }
        ICity Cities { get; }
        IState States { get; }
        IOffice Offices { get; }
        IPhone Phones { get; }
        Task<int> SaveAsync();
    }
}