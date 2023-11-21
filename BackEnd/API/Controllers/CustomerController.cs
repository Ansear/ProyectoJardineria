using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace API.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardenContext _context;

        public CustomerController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> Get()
        {
            var customer = await _unitOfWork.Customers.GetAllAsync();
            return _mapper.Map<List<CustomerDto>>(customer);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> Get(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return _mapper.Map<CustomerDto>(customer);
        }

        // Devuelve un listado con el código de cliente de aquellos clientes que realizaron algún pago en 2008. Tenga en cuenta que deberá eliminar aquellos códigos de cliente que aparezcan repetidos.Resuelva la consulta:
        // • Utilizando la función YEAR de MySQL.
        // • Utilizando la función DATE_FORMAT de MySQL.
        // • Sin utilizar ninguna de las funciones anteriores.

        [HttpGet("CustomerWithPaymentIn/{year}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IEnumerable<Customer>> GetCustomerWithPaymentIn(int year)
        {
            return await _unitOfWork.Customers.GetCustomersWithPaymentsInYear(year);
        }

        [HttpGet("CustomersWithSalesRepresentatives")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CustomerWithSalesRepresentative>>> GetCustomersWithSalesRepresentatives()
        {
            try
            {
                var customersWithSalesRepresentatives = await _context.Customers
                    .Join(
                        _context.OrderCustomerEmployees,
                        customer => customer.Id,
                        orderCustomerEmployee => orderCustomerEmployee.IdCustomer,
                        (customer, orderCustomerEmployee) => new { Customer = customer, OrderCustomerEmployee = orderCustomerEmployee }
                    )
                    .Join(
                        _context.Employees,
                        combined => combined.OrderCustomerEmployee.IdEmployee,
                        employee => employee.Id,
                        (combined, employee) => new CustomerWithSalesRepresentative
                        {
                            CustomerName = $"{combined.Customer.CustomerName} {combined.Customer.CustomerLastName}",
                            SalesRepresentativeName = $"{employee.EmployeeName} {employee.EmployeeLastName}"
                        }
                    )
                    .ToListAsync();

                if (customersWithSalesRepresentatives.Any())
                {
                    return Ok(customersWithSalesRepresentatives);
                }
                else
                {
                    return BadRequest("No se encontraron clientes con representantes de ventas.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("CustomersWithoutPaymentsWithRepresentativesAndOffices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CustomerWithRepresentativeAndOffice>>> CustomersWithoutPaymentsWithRepresentativesAndOffices()
        {
            try
            {
                var customersWithoutPayments = await _context.Customers
                    .Where(customer => !_context.Payments.Any(payment => payment.Order.OrderCustomerEmployees.Any(oce => oce.IdCustomer == customer.Id)))
                    .Select(customer => new CustomerWithRepresentativeAndOffice
                    {
                        CustomerName = $"{customer.CustomerName} {customer.CustomerLastName}",
                        RepresentativeName = _context.OfficeEmployees
                            .Where(oe => oe.IdEmployee == customer.OrderCustomerEmployees.First().IdEmployee)
                            .Select(oe => $"{oe.Employee.EmployeeName} {oe.Employee.EmployeeLastName}")
                            .FirstOrDefault(),
                        OfficeCity = _context.OfficeEmployees
                            .Where(oe => oe.IdEmployee == customer.OrderCustomerEmployees.First().IdEmployee)
                            .Select(oe => oe.Office.Address.Cities.Name)
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                if (customersWithoutPayments.Any())
                {
                    return Ok(customersWithoutPayments);
                }
                else
                {
                    return BadRequest("No se encontraron clientes sin pagos.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("ProductRangesByCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CustomerProductRange>>> GetProductRangesByCustomer()
        {
            try
            {
                var productRangesByCustomer = await _context.Customers
                    .SelectMany(customer => customer.OrderCustomerEmployees
                        .SelectMany(oce => oce.Order.OrderDetails
                            .Select(od => od.Product.Gamma.TextDescription)
                            .Distinct()
                            .Select(productRange => new CustomerProductRange
                            {
                                CustomerName = $"{customer.CustomerName} {customer.CustomerLastName}",
                                ProductRange = productRange
                            })
                        )
                    )
                    .ToListAsync();

                if (productRangesByCustomer.Any())
                {
                    return Ok(productRangesByCustomer);
                }
                else
                {
                    return BadRequest("No se encontraron datos de gamas de productos por cliente.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("CustomersWithoutPayments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersWithoutPayments()
        {
            try
            {
                var customersWithoutPayments = await _context.Customers
                    .Where(customer => !customer.OrderCustomerEmployees.Any(oce => oce.Order.Payment != null))
                    .Select(customer => new CustomerDto
                    {
                        Id = customer.Id,
                        CustomerName = customer.CustomerName,
                        CustomerLastName = customer.CustomerLastName
                    })
                    .ToListAsync();

                if (customersWithoutPayments.Any())
                {
                    return Ok(customersWithoutPayments);
                }
                else
                {
                    return BadRequest("No se encontraron clientes sin pagos.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Customer>> Post(CustomerDto CustomerDto)
        {
            var customer = _mapper.Map<Customer>(CustomerDto);
            this._unitOfWork.Customers.Add(customer);
            await _unitOfWork.SaveAsync();

            if (customer == null)
            {
                return BadRequest();
            }
            CustomerDto.Id = customer.Id;
            return CreatedAtAction(nameof(Post), new { id = CustomerDto.Id }, CustomerDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> Put(int id, [FromBody] CustomerDto CustomerDto)
        {
            if (CustomerDto.Id == 0)
            {
                CustomerDto.Id = id;
            }

            if (CustomerDto.Id != id)
            {
                return BadRequest();
            }

            if (CustomerDto == null)
            {
                return NotFound();
            }

            var customer = _mapper.Map<Customer>(CustomerDto);
            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.SaveAsync();
            return CustomerDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            _unitOfWork.Customers.Remove(customer);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("GetCustomerByCountry/{countryname}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomerByCountry(string countryname)
        {
            var result = await _unitOfWork.Customers.GetCustomerByCountry(countryname);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<CustomerDto>>(result);
        }

        [HttpGet("GetCustomerWithEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ShowCustomerEmployeeDto>>> GetCustomerEmployee()
        {
            var result = await _context.OrderCustomerEmployees
                    .Include(oce => oce.Customer)
                    .Include(oce => oce.Employee)
                    .Where(oce => oce.Customer != null && oce.Employee != null)
                    .Select(oce => new ShowCustomerEmployeeDto
                    {
                        CustomerName = oce.Customer.CustomerName,
                        SalesRepresentativeName = oce.Employee.EmployeeName,
                        SalesRepresentativeLastName = oce.Employee.EmployeeLastName
                    })
                    .ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<ShowCustomerEmployeeDto>>(result);
        }

        [HttpGet("GetCustomerEmployeeAndCity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ShowCustomerEmployeeDto>>> GetCustomerEmployeeAndCity()
        {
            var result = await _context.OrderCustomerEmployees
                .Include(oce => oce.Customer)
                .Include(oce => oce.Employee)
                    .ThenInclude(e => e.OfficeEmployee)
                        .ThenInclude(oe => oe.Office)
                            .ThenInclude(o => o.Address)
                                .ThenInclude(a => a.Cities)
                .SelectMany(oce => oce.Employee.OfficeEmployee
                    .Select(oe => new CustomerEmployeeAndCity
                    {
                        CustomerName = oce.Customer.CustomerName,
                        CustomerLastName = oce.Customer.CustomerLastName,
                        SalesRepresentativeName = oe.Employee.EmployeeName,
                        SalesRepresentativeLastName = oe.Employee.EmployeeLastName,
                        OfficeCity = oe.Office.Address.Cities.Name
                    })
                )
                .ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetCustomersWithLateDelivering")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersWithLateDelivering()
        {
            var result = await _context.OrderCustomerEmployees
                .Where(oce => oce.Order.DeliveryDate > oce.Order.ExpectedDate)
                .Select(oce => oce.Customer.CustomerName + " " + oce.Customer.CustomerLastName)
                .Distinct() 
                .ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetAllCountCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetCustomersCount()
        {
            var result = await _context.Customers.CountAsync();
            return Ok(result);
        }

        [HttpGet("CustomersInMadridWithSalesRepresentatives")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CustomerDto>> GetCustomersInMadridWithSalesRepresentatives()
        {

            var customers = _context.Customers
                .Join(
                    _context.Address,
                    customer => customer.AddressId,
                    address => address.Id,
                    (customer, address) => new { Customer = customer, Address = address })
                .Join(
                    _context.Cities,
                    customerAddress => customerAddress.Address.IdCity,
                    city => city.Id,
                    (customerAddress, city) => new { CustomerAddress = customerAddress, City = city })
                .Join(
                    _context.OrderCustomerEmployees,
                    customerCity => customerCity.CustomerAddress.Customer.Id,
                    orderCustomerEmployee => orderCustomerEmployee.IdCustomer,
                    (customerCity, orderCustomerEmployee) => new { CustomerCity = customerCity, OrderCustomerEmployee = orderCustomerEmployee })
                .Where(
                    combined => combined.CustomerCity.City.Name == "Madrid" &&
                                (combined.OrderCustomerEmployee.IdEmployee == "11" || combined.OrderCustomerEmployee.IdEmployee == "30"))
                .Select(
                    combined => new
                    {
                        Id = combined.CustomerCity.CustomerAddress.Customer.Id,
                        CustomerName = combined.CustomerCity.CustomerAddress.Customer.CustomerName,
                        CustomerLastName = combined.CustomerCity.CustomerAddress.Customer.CustomerLastName,
                        CustomerPhoneId = combined.CustomerCity.CustomerAddress.Customer.CustomerPhoneId,
                        CustomerFax = combined.CustomerCity.CustomerAddress.Customer.CustomerFax,
                        AddressId = combined.CustomerCity.CustomerAddress.Customer.AddressId,
                        CreditLimit = combined.CustomerCity.CustomerAddress.Customer.CreditLimit,
                        TypePersonId = combined.CustomerCity.CustomerAddress.Customer.TypePersonId,
                        IdUser = combined.CustomerCity.CustomerAddress.Customer.IdUser
                    })
                .ToList();

            if (customers == null || !customers.Any())
            {
                return NotFound("No customers found that meet the criteria.");
            }

            return Ok(customers);
        }

        [HttpGet("CustomerCountByCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetCustomerCountByCountry()
        {
            var customerCountByCountry = _context.Cities
                .GroupJoin(
                    _context.Address,
                    city => city.Id,
                    address => address.IdCity,
                    (city, addresses) => new { City = city, Addresses = addresses }
                )
                .SelectMany(
                    x => x.Addresses.DefaultIfEmpty(),
                    (cityGroup, address) => new { City = cityGroup.City, Address = address }
                )
                .GroupJoin(
                    _context.Customers,
                    x => x.Address.Id,
                    customer => customer.AddressId,
                    (x, customers) => new { City = x.City, Customers = customers }
                )
                .Select(x => new
                {
                    CountryName = x.City.Name,
                    CustomerCount = x.Customers.Count()
                })
                .ToList();

            if (customerCountByCountry == null || !customerCountByCountry.Any())
            {
                return NotFound("No customer counts by country found.");
            }

            return Ok(customerCountByCountry);
        }

        [HttpGet("CustomerWithHighestCreditLimit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<string> GetCustomerWithHighestCreditLimit()
        {
            var CreditLimit = _context.Customers
                .OrderByDescending(c => c.CreditLimit)
                .FirstOrDefault();

            if (CreditLimit == null)
            {
                return NotFound("No customers found.");
            }

            return Ok($"Customer:Name= {CreditLimit.CustomerName}, LastName= {CreditLimit.CustomerLastName}");
        }

        [HttpGet("WithoutPayments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult WithoutPayments()
        {
            var customersWithoutPayments = _context.Customers
                .Where(customer => !_context.OrderCustomerEmployees
                    .Any(oce => oce.IdCustomer == customer.Id && oce.Order != null && oce.Order.Payment != null))
                .Select(customer => new
                {
                    Id = customer.Id,
                    CustomerName = customer.CustomerName,
                    CustomerLastName = customer.CustomerLastName,
                    CustomerPhoneId = customer.CustomerPhoneId,
                    CustomerFax = customer.CustomerFax,
                    AddressId = customer.AddressId,
                    CreditLimit = customer.CreditLimit,
                    TypePersonId = customer.TypePersonId,
                    IdUser = customer.IdUser
                })
                .ToList();

            if (customersWithoutPayments == null || !customersWithoutPayments.Any())
            {
                return NotFound("No customers found without payments.");
            }

            return Ok(customersWithoutPayments);
        }

        [HttpGet("WithPayments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> WithPayments()
        {
            var customersWithPayments = await _context.Customers
            .Where(customer => _context.OrderCustomerEmployees
                .Any(oce => oce.IdCustomer == customer.Id && oce.Order.Payment != null))
            .Select(customer => new
            {
                Id = customer.Id,
                CustomerName = customer.CustomerName,
                CustomerLastName = customer.CustomerLastName,
                CustomerPhoneId = customer.CustomerPhoneId,
                CustomerFax = customer.CustomerFax,
                AddressId = customer.AddressId,
                CreditLimit = customer.CreditLimit,
                TypePersonId = customer.TypePersonId,
                IdUser = customer.IdUser
            })
            .ToListAsync();

            if (customersWithPayments == null || !customersWithPayments.Any())
            {
                return NotFound("No customers found with payments.");
            }

            return Ok(customersWithPayments);
        }

        [HttpGet("OrdersIn2008")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomersWithOrdersIn2008()
        {
            var customersWithOrdersIn2008 = await _context.Customers
                .Where(customer => _context.OrderCustomerEmployees
                    .Any(oce => oce.IdCustomer == customer.Id &&
                                oce.Order.OrderDate.Year == 2008))
                .OrderBy(customer => customer.CustomerName)
                .Select(customer => new
                {
                    Id = customer.Id,
                    CustomerName = customer.CustomerName,
                    CustomerLastName = customer.CustomerLastName,
                    CustomerPhoneId = customer.CustomerPhoneId,
                    CustomerFax = customer.CustomerFax,
                    AddressId = customer.AddressId,
                    CreditLimit = customer.CreditLimit,
                    TypePersonId = customer.TypePersonId,
                    IdUser = customer.IdUser
                })
                .ToListAsync();

            if (customersWithOrdersIn2008 == null || !customersWithOrdersIn2008.Any())
            {
                return NotFound("No customers found with orders in 2008.");
            }

            return Ok(customersWithOrdersIn2008);
        }

    }
}