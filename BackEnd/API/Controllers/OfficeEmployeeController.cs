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
    public class OfficeEmployeeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardenContext _context;

        public OfficeEmployeeController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OfficeEmployeeDto>>> Get()
        {
            var OfficeEmployeees = await _unitOfWork.OfficeEmployees.GetAllAsync();
            return _mapper.Map<List<OfficeEmployeeDto>>(OfficeEmployeees);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OfficeEmployeeDto>> Get(string id)
        {
            var OfficeEmployeees = await _unitOfWork.OfficeEmployees.GetByIdAsync(id);

            if (OfficeEmployeees == null)
            {
                return NotFound();
            }

            return _mapper.Map<OfficeEmployeeDto>(OfficeEmployeees);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OfficeEmployee>> Post(OfficeEmployeeDto OfficeEmployeeDto)
        {
            var OfficeEmployee = _mapper.Map<OfficeEmployee>(OfficeEmployeeDto);
            this._unitOfWork.OfficeEmployees.Add(OfficeEmployee);
            await _unitOfWork.SaveAsync();

            if (OfficeEmployee == null)
            {
                return BadRequest();
            }
            OfficeEmployeeDto.IdOfficeEmployee = OfficeEmployee.IdOffice;
            return CreatedAtAction(nameof(Post), new { id = OfficeEmployeeDto.IdOfficeEmployee }, OfficeEmployeeDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OfficeEmployeeDto>> Put(string id, [FromBody] OfficeEmployeeDto OfficeEmployeeDto)
        {
            if (OfficeEmployeeDto.IdOfficeEmployee == null)
            {
                OfficeEmployeeDto.IdOfficeEmployee = id;
            }

            if (OfficeEmployeeDto.IdOfficeEmployee != id)
            {
                return BadRequest();
            }

            if (OfficeEmployeeDto == null)
            {
                return NotFound();
            }

            var OfficeEmployee = _mapper.Map<OfficeEmployee>(OfficeEmployeeDto);
            _unitOfWork.OfficeEmployees.Update(OfficeEmployee);
            await _unitOfWork.SaveAsync();
            return OfficeEmployeeDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var OfficeEmployeees = await _unitOfWork.OfficeEmployees.GetByIdAsync(id);

            if (OfficeEmployeees == null)
            {
                return NotFound();
            }

            _unitOfWork.OfficeEmployees.Remove(OfficeEmployeees);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}