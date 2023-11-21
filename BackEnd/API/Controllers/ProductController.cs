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
    public class ProductController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardenContext _context;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
        {
            var nombreVariable = await _unitOfWork.Products.GetAllAsync();
            return _mapper.Map<List<ProductDto>>(nombreVariable);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> Get(string id)
        {
            var nombreVariable = await _unitOfWork.Products.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProductDto>(nombreVariable);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> Post(ProductDto ProductDto)
        {
            var nombreVariable = _mapper.Map<Product>(ProductDto);
            this._unitOfWork.Products.Add(nombreVariable);
            await _unitOfWork.SaveAsync();

            if (nombreVariable == null)
            {
                return BadRequest();
            }
            ProductDto.Id = nombreVariable.Id;
            return CreatedAtAction(nameof(Post), new { id = ProductDto.Id }, ProductDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> Put(string id, [FromBody] ProductDto ProductDto)
        {
            if (ProductDto.Id == "" || ProductDto.Id == null)
            {
                ProductDto.Id = id;
            }

            if (ProductDto.Id != id)
            {
                return BadRequest();
            }

            if (ProductDto == null)
            {
                return NotFound();
            }

            var nombreVariable = _mapper.Map<Product>(ProductDto);
            _unitOfWork.Products.Update(nombreVariable);
            await _unitOfWork.SaveAsync();
            return ProductDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var nombreVariable = await _unitOfWork.Products.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            _unitOfWork.Products.Remove(nombreVariable);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("GetProductByGamma&Stock/{idgamma}/{instock}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetDeliveryByMonth(string idgamma, int stock)
        {
            var result = await _context.Products
                                .Where(p => p.Gamma != null && p.Gamma.Id == idgamma && p.InStockQuantity > stock)
                                .OrderByDescending(p => p.ProductSalesPrice).ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<ProductDto>>(result);
        }

        [HttpGet("GetProductNeverOrdered")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsNeverOrdered(string idgamma, int stock)
        {
            var orderedProductIds = await _context.OrderDetails
                .Select(od => od.Product.Id) // Obtén los IDs de productos en órdenes
                .Distinct()
                .ToListAsync();

            var result = _context.Products
                .Where(p => !orderedProductIds.Contains(p.Id)) // Filtra productos que no están en órdenes
                .ToList();
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<ProductDto>>(result);
        }

        [HttpGet("ProductsCountInOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<object>>> GetProductsCountInOrders()
        {
            var result = await _context.OrderDetails
                .GroupBy(od => od.OrderCode)
                .Select(group => new
                {
                    OrderCode = group.Key,
                    ProductCount = group.Select(od => od.ProductCode).Distinct().Count()
                })
                .ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("MostExpensiveProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetMostExpensiveProductAsync()
        {
            var mostExpensiveProduct = await _context.Products
                .OrderByDescending(p => p.ProductSalesPrice)
                .FirstOrDefaultAsync();

            if (mostExpensiveProduct == null)
            {
                return NotFound();
            }

            return Ok(mostExpensiveProduct.ProductName);
        }

        [HttpGet("ProductWithMostStock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductWithMostStockAsync()
        {
            var productWithMostStock = await _context.Products
                .OrderByDescending(p => p.InStockQuantity)
                .FirstOrDefaultAsync();

            if (productWithMostStock == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProductDto>(productWithMostStock);
        }
    }
}