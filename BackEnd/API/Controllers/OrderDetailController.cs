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
    public class OrderDetailController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardenContext _context;

        public OrderDetailController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> Get()
        {
            var nombreVariable = await _unitOfWork.OrderDetails.GetAllAsync();
            return _mapper.Map<List<OrderDetailDto>>(nombreVariable);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDetailDto>> Get(int id)
        {
            var nombreVariable = await _unitOfWork.OrderDetails.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            return _mapper.Map<OrderDetailDto>(nombreVariable);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDetail>> Post(OrderDetailDto OrderDetailDto)
        {
            var nombreVariable = _mapper.Map<OrderDetail>(OrderDetailDto);
            this._unitOfWork.OrderDetails.Add(nombreVariable);
            await _unitOfWork.SaveAsync();

            if (nombreVariable == null)
            {
                return BadRequest();
            }
            OrderDetailDto.Id = nombreVariable.Id;
            return CreatedAtAction(nameof(Post), new { id = OrderDetailDto.Id }, OrderDetailDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDetailDto>> Put(int id, [FromBody] OrderDetailDto OrderDetailDto)
        {
            if (OrderDetailDto.Id == 0)
            {
                OrderDetailDto.Id = id;
            }

            if (OrderDetailDto.Id != id)
            {
                return BadRequest();
            }

            if (OrderDetailDto == null)
            {
                return NotFound();
            }

            var nombreVariable = _mapper.Map<OrderDetail>(OrderDetailDto);
            _unitOfWork.OrderDetails.Update(nombreVariable);
            await _unitOfWork.SaveAsync();
            return OrderDetailDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var nombreVariable = await _unitOfWork.OrderDetails.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            _unitOfWork.OrderDetails.Remove(nombreVariable);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("Top20BestSellingProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetTop20BestSellingProducts()
        {
            var top20BestSellingProducts = _context.OrderDetails
                .GroupBy(od => od.ProductCode)
                .Select(group => new 
                {
                    ProductCode = group.Key,
                    ProductName = group.First().Product.ProductName,
                    TotalUnitsSold = group.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.TotalUnitsSold)
                .Take(20)
                .ToList();

            if (top20BestSellingProducts == null || !top20BestSellingProducts.Any())
            {
                return NotFound("No best-selling products found.");
            }

            return Ok(top20BestSellingProducts);
        }

        [HttpGet("TotalBilling")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> GetTotalBillingAsync()
        {
            var result = await _context.OrderDetails
                .Include(od => od.Product)
                .GroupBy(od => od.OrderCode)
                .Select(group => new
                {
                    OrderCode = group.Key,
                    BaseImponible = group.Sum(od => od.UnitPrice * od.Quantity),
                    IVA = Math.Round((double)(group.Sum(od => od.UnitPrice * od.Quantity) * 0.21m), 2),
                    TotalFacturado = Math.Round((double)(group.Sum(od => od.UnitPrice * od.Quantity) * 1.21m), 2)
                })
                .ToListAsync();

            var totalBilling = new
            {
                TotalBaseImponible = Math.Round((double)result.Sum(r => r.BaseImponible), 2),
                TotalIVA = Math.Round((double)result.Sum(r => r.IVA), 2),
                TotalFacturado = Math.Round((double)result.Sum(r => r.TotalFacturado), 2)
            };
            if (result == null)
            {
                return NotFound();
            }
            return Ok(totalBilling);
        }
    }
}