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

namespace API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

            if(ProductDto.Id != id)
            {
                return BadRequest();
            }

            if(ProductDto == null)
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
    }
}