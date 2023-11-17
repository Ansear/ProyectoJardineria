using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APP.Interfaces;
using APP.Repository;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace APP.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IBoss _bosses;
        private ICustomer _customers;
        private IEmployee _employees;
        private IOfficeEmployee _officeEmployees;
        private ISupplier _suppliers;
        private ITypePerson _typePersons;
        private readonly GardenContext _context;

        public UnitOfWork(GardenContext context)
        {
            _context = context;
        }

        public IBoss Bosses
        {
            get
            {
                if (_bosses == null)
                {
                    _bosses = new BossRepository(_context);
                }
                return _bosses;
            }
        }

        public ICustomer Customers
        {
            get
            {
                if (_customers == null)
                {
                    _customers = new CustomerRepository(_context);
                }
                return _customers;
            }
        }

        public IEmployee Employees
        {
            get
            {
                if (_employees == null)
                {
                    _employees = new EmployeeRepository(_context);
                }
                return _employees;
            }
        }

        public IOfficeEmployee OfficeEmployees
        {
            get
            {
                if (_officeEmployees == null)
                {
                    _officeEmployees = new OfficeEmployeeRepository(_context);
                }
                return _officeEmployees;
            }
        }

        public ISupplier Suppliers
        {
            get
            {
                if (_suppliers == null)
                {
                    _suppliers = new SupplierRepository(_context);
                }
                return _suppliers;
            }
        }

        public ITypePerson TypePersons
        {
            get
            {
                if (_typePersons == null)
                {
                    _typePersons = new TypePersonRepository(_context);
                }
                return _typePersons;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}