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
    public class EmployeeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardenContext _context;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> Get()
        {
            var Employeees = await _unitOfWork.Employees.GetAllAsync();
            return _mapper.Map<List<EmployeeDto>>(Employeees);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDto>> Get(string id)
        {
            var Employeees = await _unitOfWork.Employees.GetByIdAsync(id);

            if (Employeees == null)
            {
                return NotFound();
            }

            return _mapper.Map<EmployeeDto>(Employeees);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> Post(EmployeeDto EmployeeDto)
        {
            var Employee = _mapper.Map<Employee>(EmployeeDto);
            this._unitOfWork.Employees.Add(Employee);
            await _unitOfWork.SaveAsync();

            if (Employee == null)
            {
                return BadRequest();
            }
            EmployeeDto.IdEmployee = Employee.Id;
            return CreatedAtAction(nameof(Post), new { id = EmployeeDto.IdEmployee }, EmployeeDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDto>> Put(string id, [FromBody] EmployeeDto EmployeeDto)
        {
            if (EmployeeDto.IdEmployee == null)
            {
                EmployeeDto.IdEmployee = id;
            }

            if (EmployeeDto.IdEmployee != id)
            {
                return BadRequest();
            }

            if (EmployeeDto == null)
            {
                return NotFound();
            }

            var Employee = _mapper.Map<Employee>(EmployeeDto);
            _unitOfWork.Employees.Update(Employee);
            await _unitOfWork.SaveAsync();
            return EmployeeDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var Employeees = await _unitOfWork.Employees.GetByIdAsync(id);

            if (Employeees == null)
            {
                return NotFound();
            }

            _unitOfWork.Employees.Remove(Employeees);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("GetByBossId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetByBossId(string id)
        {
            var result = await _unitOfWork.Employees.GetEmployeesByIdBoss(id);
            return Ok(result);
        }
    }
}