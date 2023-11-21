using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace APP.Repository
{
    public class CustomerRepository : GenericRepositoryInt<Customer>, ICustomer
    {
        private readonly GardenContext _context;

        public CustomerRepository(GardenContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetCustomerByCountry(string countryName)
        {
            countryName = countryName.ToLower();
            var result = await _context.Customers
                .Where(e => e.Address != null &&
                            e.Address.Cities != null &&
                            e.Address.Cities.States != null &&
                            e.Address.Cities.States.Countries != null &&
                            e.Address.Cities.States.Countries.Name.ToLower() == countryName)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Customer>> GetCustomersWithPaymentsInYear(int year)
        {
            var customerIds = await _context.Payments
                .Where(payment => payment.PaymentDate.Year == year && payment.PaymentDate.Month == 1 && payment.PaymentDate.Day == 1) // Filtrar solo por la fecha sin tener en cuenta la hora
                .Select(payment => payment.Order.OrderCustomerEmployees.First().IdCustomer)
                .Distinct()
                .ToListAsync();

            var customers = await _context.Customers
                .Where(customer => customerIds.Contains(customer.Id))
                .ToListAsync();

            return customers;
        }


    }
}