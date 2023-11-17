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
        Task<int> SaveAsync();
    }
}