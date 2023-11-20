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

        // Devuelve un listado con el código de cliente de aquellos clientes que realizaron algún pago en 2008. Tenga en cuenta que deberá eliminar aquellos códigos de cliente que aparezcan repetidos.Resuelva la consulta:
        // • Utilizando la función YEAR de MySQL.
        // • Utilizando la función DATE_FORMAT de MySQL.
        // • Sin utilizar ninguna de las funciones anteriores.

        // [HttpGet("CustomerWithPaymentIn2008")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        // public async Task<IEnumerable<Customer>> GetCustomerWithPaymentIn2008()
        // {
        //     return await _unitOfWork.Customers.GetCustomerWithPaymentIn2008();
        // }

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
    }
}