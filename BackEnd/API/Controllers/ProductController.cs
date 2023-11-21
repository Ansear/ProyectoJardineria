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

        [HttpGet("MinMaxProductSalesPrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetMinMaxProductSalesPrice()
        {
            var minMaxPrices = _context.Products
            .OrderByDescending(p => p.ProductSalesPrice)
            .Select(p => new
            {
                ProductName = p.ProductName,
                ProductSalesPrice = p.ProductSalesPrice
            })
            .Take(2)
            .ToList();

            if (minMaxPrices == null || !minMaxPrices.Any())
            {
                return NotFound("No products found.");
            }

            return Ok(minMaxPrices);
        }

        [HttpGet("LeastStock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductDto> GetLeastStock()
        {
            var productWithLeastStock = _context.Products
                .OrderBy(product => product.InStockQuantity)
                .FirstOrDefault();

            if (productWithLeastStock == null)
            {
                return NotFound("No products found.");
            }

            var productDto = new 
            {
                ProductName = productWithLeastStock.ProductName,
                ProductDimensions = productWithLeastStock.ProductDimensions,
                ProductDescription = productWithLeastStock.ProductDescription,
                ProductSalesPrice = productWithLeastStock.ProductSalesPrice,
                InStockQuantity = productWithLeastStock.InStockQuantity,
                SupplierPrice = productWithLeastStock.SupplierPrice,
                IdGamma = productWithLeastStock.IdGamma
            };

            return Ok(productDto);
        }
    }
}