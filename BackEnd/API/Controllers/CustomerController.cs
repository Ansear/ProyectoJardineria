using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomerByCountry(string countryname)
        {
            var result = await _unitOfWork.Customers.GetCustomerByCountry(countryname);
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
    }
}