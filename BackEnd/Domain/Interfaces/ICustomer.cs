using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICustomer : IGenericRepositoryInt<Customer>
    {
        Task<IEnumerable<Customer>> GetCustomerByCountry(string countryName);
        Task<IEnumerable<Customer>> GetCustomersWithPaymentsInYear(int year);
    }
}