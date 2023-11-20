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
        private ICustomer _customers;
        private IEmployee _employees;
        private IOfficeEmployee _officeEmployees;
        private ISupplier _suppliers;
        private ITypePerson _typePersons;
        public IAddress _address;
        public ICountry _countries;
        public IState _state;
        public ICity _cities;
        public IOffice _offices;
        public IPhone _phones;

        private readonly GardenContext _context;

        public UnitOfWork(GardenContext context)
        {
            _context = context;
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
        public IAddress Address
        {
            get
            {
                if (_address == null)
                {
                    _address = new AddressRepository(_context);
                }
                return _address;
            }
        }
        public ICountry Countries
        {
            get
            {
                if (_countries == null)
                {
                    _countries = new CountryRepository(_context);
                }
                return _countries;
            }
        }
        public IState States
        {
            get
            {
                if (_state == null)
                {
                    _state = new StateRepository(_context);
                }
                return _state;
            }
        }
        public ICity Cities
        {
            get
            {
                if (_cities == null)
                {
                    _cities = new CityRepository(_context);
                }
                return _cities;
            }
        }
        public IOffice Offices
        {
            get
            {
                if (_offices == null)
                {
                    _offices = new OfficeRepository(_context);
                }
                return _offices;
            }
        }
        public IPhone Phones
        {
            get
            {
                if (_phones == null)
                {
                    _phones = new PhoneRepository(_context);
                }
                return _phones;
            }
        }
        private ProductRepository _product;  
        public IProduct Products
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_context);
                }
                return _product;
            }
        }

        private OrderRepository _order;  
        public IOrder Orders
        {
            get
            {
                if (_order == null)
                {
                    _order = new OrderRepository(_context);
                }
                return _order;
            }
        }

        private OrderDetailRepository _orderDetail; 
        public IOrderDetail OrderDetails
        {
            get
            {
                if (_orderDetail == null)
                {
                    _orderDetail = new OrderDetailRepository(_context);
                }
                return _orderDetail;
            }
        }

        private PaymentRepository _payment; 
        public IPayment Payments
        {
            get
            {
                if (_payment == null)
                {
                    _payment = new PaymentRepository(_context);
                }
                return _payment;
            }
        }

        private PaymentFormRepository _paymentForm; 
        public IPaymentForm PaymentForms
        {
            get
            {
                if (_paymentForm == null)
                {
                    _paymentForm = new PaymentFormRepository(_context);
                }
                return _paymentForm;
            }
        }

        private ProductGammaRepository _productGamma; 
        public IProductGamma ProductGammas
        {
            get
            {
                if (_productGamma == null)
                {
                    _productGamma = new ProductGammaRepository(_context);
                }
                return _productGamma;
            }
        }

        private UserRepository _user;
        public IUser Users
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context);
                }
                return _user;
            }
        }

        private RolRepository _rol; 
        public IRol Rols
        {
            get
            {
                if (_rol == null)
                {
                    _rol = new RolRepository(_context);
                }
                return _rol;
            }
        }
        private OrderStatusRepository _orderStatus; 
        public IStatusOrder StatusOrder
        {
            get
            {
                if (_orderStatus == null)
                {
                    _orderStatus = new OrderStatusRepository(_context);
                }
                return _orderStatus;
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