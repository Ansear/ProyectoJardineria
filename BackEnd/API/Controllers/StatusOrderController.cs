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
    public class StatusOrderController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardenContext _context;

        public StatusOrderController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<StatusOrderDto>>> Get()
        {
            var result = await _unitOfWork.Customers.GetAllAsync();
            return _mapper.Map<List<StatusOrderDto>>(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StatusOrderDto>> Get(int id)
        {
            var result = await _unitOfWork.StatusOrder.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return _mapper.Map<StatusOrderDto>(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StatusOrder>> Post(StatusOrderDto statusOrderDto)
        {
            var result = _mapper.Map<StatusOrder>(statusOrderDto);
            this._unitOfWork.StatusOrder.Add(result);
            await _unitOfWork.SaveAsync();

            if (result == null)
            {
                return BadRequest();
            }
            statusOrderDto.Id = result.Id;
            return CreatedAtAction(nameof(Post), new { id = statusOrderDto.Id }, statusOrderDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StatusOrderDto>> Put(int id, [FromBody] StatusOrderDto StatusOrderDto)
        {
            if (StatusOrderDto.Id == 0)
            {
                StatusOrderDto.Id = id;
            }

            if (StatusOrderDto.Id != id)
            {
                return BadRequest();
            }

            if (StatusOrderDto == null)
            {
                return NotFound();
            }

            var statusOrder = _mapper.Map<StatusOrder>(StatusOrderDto);
            _unitOfWork.StatusOrder.Update(statusOrder);
            await _unitOfWork.SaveAsync();
            return StatusOrderDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.StatusOrder.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            _unitOfWork.StatusOrder.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("Status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<string>> GetStatusOrder()
        {
            var estados = _context.StatusOrders
        .Select(status => status.Description)
        .Distinct()
        .ToList();

            if (estados == null || !estados.Any())
            {
                return NotFound("No se encontraron estados de pedidos.");
            }

            return Ok(estados);
        }
    }
}