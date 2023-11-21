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
                            .Select(oe => oe.Office.Address.Cities.Name) // Accediendo a la propiedad Name de City
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
                            .Select(od => od.Product.Gamma.TextDescription) // Accede a la propiedad TextDescription de ProductGamma
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
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersWithoutPayments()
        {
            try
            {
                var customersWithoutPayments = await _context.Customers
                    .Where(customer => !customer.OrderCustomerEmployees.Any(oce => oce.Order.Payment != null))
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
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomerByCountry(string countryname)
        {
            var result = await _unitOfWork.Customers.GetCustomerByCountry(countryname);
            return Ok(result);
        }
    }
}