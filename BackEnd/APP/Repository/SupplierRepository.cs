using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace APP.Repository
{
    public class SupplierRepository : GenericRepositoryInt<Supplier>, ISupplier
    {
        private readonly GardenContext _context;

        public SupplierRepository(GardenContext context) : base(context)
        {
            _context = context;
        }
    }
}