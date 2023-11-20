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
    public class EmployeeRepository : GenericRepositoryVarchar<Employee>, IEmployee
    {
        private readonly GardenContext _context;

        public EmployeeRepository(GardenContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByIdBoss(string id)
        {
            return await _context.Employees.Where(e => e.IdBoss == id).ToListAsync();;
        }
    }
}