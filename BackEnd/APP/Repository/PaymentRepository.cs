using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace APP.Repository;
public class PaymentRepository : GenericRepositoryInt<Payment>, IPayment
{
    private readonly GardenContext _context;
    public PaymentRepository(GardenContext context) : base(context)
    {
        _context = context;
    }
}
