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
    public class ProductGammaController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductGammaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductGammaDto>>> Get()
        {
            var nombreVariable = await _unitOfWork.ProductGammas.GetAllAsync();
            return _mapper.Map<List<ProductGammaDto>>(nombreVariable);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductGammaDto>> Get(string id)
        {
            var nombreVariable = await _unitOfWork.ProductGammas.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProductGammaDto>(nombreVariable);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductGamma>> Post(ProductGammaDto ProductGammaDto)
        {
            var nombreVariable = _mapper.Map<ProductGamma>(ProductGammaDto);
            this._unitOfWork.ProductGammas.Add(nombreVariable);
            await _unitOfWork.SaveAsync();

            if (nombreVariable == null)
            {
                return BadRequest();
            }
            ProductGammaDto.Id = nombreVariable.Id;
            return CreatedAtAction(nameof(Post), new { id = ProductGammaDto.Id }, ProductGammaDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductGammaDto>> Put(string id, [FromBody] ProductGammaDto ProductGammaDto)
        {
            if (ProductGammaDto.Id == "" || ProductGammaDto.Id == null)
            {
                ProductGammaDto.Id = id;
            }

            if(ProductGammaDto.Id != id)
            {
                return BadRequest();
            }

            if(ProductGammaDto == null)
            {
                return NotFound();
            }

            var nombreVariable = _mapper.Map<ProductGamma>(ProductGammaDto);
            _unitOfWork.ProductGammas.Update(nombreVariable);
            await _unitOfWork.SaveAsync();
            return ProductGammaDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var nombreVariable = await _unitOfWork.ProductGammas.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            _unitOfWork.ProductGammas.Remove(nombreVariable);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}