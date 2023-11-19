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
    public class SupplierController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardenContext _context;

        public SupplierController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> Get()
        {
            var Supplier = await _unitOfWork.Suppliers.GetAllAsync();
            return _mapper.Map<List<SupplierDto>>(Supplier);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierDto>> Get(int id)
        {
            var Supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);

            if (Supplier == null)
            {
                return NotFound();
            }

            return _mapper.Map<SupplierDto>(Supplier);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Supplier>> Post(SupplierDto SupplierDto)
        {
            var Supplier = _mapper.Map<Supplier>(SupplierDto);
            this._unitOfWork.Suppliers.Add(Supplier);
            await _unitOfWork.SaveAsync();

            if (Supplier == null)
            {
                return BadRequest();
            }
            SupplierDto.IdSupplier = Supplier.Id;
            return CreatedAtAction(nameof(Post), new { id = SupplierDto.IdSupplier }, SupplierDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierDto>> Put(int id, [FromBody] SupplierDto SupplierDto)
        {
            if (SupplierDto.IdSupplier == 0)
            {
                SupplierDto.IdSupplier = id;
            }

            if (SupplierDto.IdSupplier != id)
            {
                return BadRequest();
            }

            if (SupplierDto == null)
            {
                return NotFound();
            }

            var Supplier = _mapper.Map<Supplier>(SupplierDto);
            _unitOfWork.Suppliers.Update(Supplier);
            await _unitOfWork.SaveAsync();
            return SupplierDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var Supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);

            if (Supplier == null)
            {
                return NotFound();
            }

            _unitOfWork.Suppliers.Remove(Supplier);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}