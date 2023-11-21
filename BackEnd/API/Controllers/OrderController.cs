using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace API.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardenContext _context;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
        {
            var nombreVariable = await _unitOfWork.Orders.GetAllAsync();
            return _mapper.Map<List<OrderDto>>(nombreVariable);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> Get(int id)
        {
            var nombreVariable = await _unitOfWork.Orders.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            return _mapper.Map<OrderDto>(nombreVariable);
        }

        [HttpGet("RejectedOrdersInYear/{year}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Order>>> GetRejectedOrdersInYear(int year)
        {
            try
            {
                var rejectedOrders = await _context.Orders
                    .Where(order => order.DeliveryDate.Year == year && order.StatusOrder.Description == "Rejected")
                    .ToListAsync();

                if (rejectedOrders.Any())
                {
                    return Ok(rejectedOrders);
                }
                else
                {
                    return BadRequest($"No se encontraron pedidos rechazados en el año {year}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("OrdersCountByStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OrdersCountByStatusDto>>> GetOrdersCountByStatus()
        {
            try
            {
                var ordersCountByStatus = await _context.Orders
                    .GroupBy(order => order.StatusOrder.Description)
                    .Select(group => new OrdersCountByStatusDto
                    {
                        Status = group.Key,
                        OrdersCount = group.Count()
                    })
                    .OrderByDescending(result => result.OrdersCount)
                    .ToListAsync();

                if (ordersCountByStatus.Any())
                {
                    return Ok(ordersCountByStatus);
                }
                else
                {
                    return BadRequest("No se encontraron datos de cantidad de pedidos por estado.");
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
        public async Task<ActionResult<Order>> Post(OrderDto OrderDto)
        {
            var nombreVariable = _mapper.Map<Order>(OrderDto);
            this._unitOfWork.Orders.Add(nombreVariable);
            await _unitOfWork.SaveAsync();

            if (nombreVariable == null)
            {
                return BadRequest();
            }
            OrderDto.Id = nombreVariable.Id;
            return CreatedAtAction(nameof(Post), new { id = OrderDto.Id }, OrderDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> Put(int id, [FromBody] OrderDto OrderDto)
        {
            if (OrderDto.Id == 0)
            {
                OrderDto.Id = id;
            }

            if (OrderDto.Id != id)
            {
                return BadRequest();
            }

            if (OrderDto == null)
            {
                return NotFound();
            }
            var nombreVariable = _mapper.Map<Order>(OrderDto);
            _unitOfWork.Orders.Update(nombreVariable);
            await _unitOfWork.SaveAsync();
            return OrderDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var nombreVariable = await _unitOfWork.Orders.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            _unitOfWork.Orders.Remove(nombreVariable);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}