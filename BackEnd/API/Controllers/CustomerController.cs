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
                .Distinct() // Elimina duplicados
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

    }
}