using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
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

        // public async Task<IEnumerable<Customer>> GetCustomerWithPaymentIn2008()
        // {
        //     // return await _context.Customers.Where(x => x.Payments.Any(y => y.PaymentDate.Year == 2008)).ToListAsync();
        // }
    }
}