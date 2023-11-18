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
        IOrder Orders { get;}
        IOrderDetail OrderDetails { get;}
        IPayment Payments { get;}
        IPaymentForm PaymentForms { get;}
        IProduct Products { get;}
        IProductGamma ProductGammas { get;}
        Task<int> SaveAsync();
    }
}