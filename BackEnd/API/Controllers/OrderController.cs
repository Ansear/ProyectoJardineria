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

        [HttpGet("OrdersDeliveredBeforeExpected")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetOrdersDeliveredBeforeExpected()
        {
            var orders = await _context.Orders
        .FromSqlRaw("SELECT Id, OrderDate, ExpectedDate, DeliveryDate, IdStatus, OrderComments, IdPayment, Total FROM `Order` WHERE DeliveryDate <= ADDDATE(ExpectedDate, -2)")
        .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found that meet the criteria.");
            }

            return Ok(orders);
        }

        

        [HttpGet("GetDeliveryOffTime")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SelectOrderWithCustomerDto>>> GetDeliveryOffTime()
        {
            var overdueOrders = _context.Orders
                .Where(o => o.DeliveryDate > o.ExpectedDate &&
                            o.OrderCustomerEmployees.Any())
                .Select(o => new SelectOrderWithCustomerDto
                {
                    OrderCode = o.Id,
                    CustomerCode = o.OrderCustomerEmployees.First().IdCustomer,
                    DeliveryDate = o.DeliveryDate,
                    ExpectedDate = o.ExpectedDate
                })
                .ToListAsync();

            return await overdueOrders;
        }

        [HttpGet("GetDeliveryByMonth/{month}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetDeliveryByMonth(int month)
        {
            var result = await _context.Orders.Where(e => e.DeliveryDate.Month == month).ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<OrderDto>>(result);
        }

        [HttpGet("GetMostMethodPayment/{year}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersWithLateDelivering(int year)
        {
            var mostUsedPaymentMethod = await _context.Payments
               .Where(p => p.PaymentDate.Year == year)
               .GroupBy(p => p.IdFormPay)
               .OrderByDescending(group => group.Count())
               .Select(group => group.Key)
               .FirstOrDefaultAsync();  // Utilizar el método asincrónico específico de EF Core

            // Aquí deberías obtener el nombre del método de pago basándote en su IdFormPay
            var result = await _context.PaymentForms
                .Where(pf => pf.Id == mostUsedPaymentMethod)
                .Select(pf => pf.PaymentFormName)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}