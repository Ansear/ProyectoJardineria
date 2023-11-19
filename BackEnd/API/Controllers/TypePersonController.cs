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
public class TypePersonController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardenContext _context;

        public TypePersonController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TypePersonDto>>> Get()
        {
            var TypePerson = await _unitOfWork.TypePersons.GetAllAsync();
            return _mapper.Map<List<TypePersonDto>>(TypePerson);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TypePersonDto>> Get(int id)
        {
            var TypePerson = await _unitOfWork.TypePersons.GetByIdAsync(id);

            if (TypePerson == null)
            {
                return NotFound();
            }

            return _mapper.Map<TypePersonDto>(TypePerson);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypePerson>> Post(TypePersonDto TypePersonDto)
        {
            var TypePerson = _mapper.Map<TypePerson>(TypePersonDto);
            this._unitOfWork.TypePersons.Add(TypePerson);
            await _unitOfWork.SaveAsync();

            if (TypePerson == null)
            {
                return BadRequest();
            }
            TypePersonDto.Id = TypePerson.Id;
            return CreatedAtAction(nameof(Post), new { id = TypePersonDto.Id }, TypePersonDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TypePersonDto>> Put(int id, [FromBody] TypePersonDto TypePersonDto)
        {
            if (TypePersonDto.Id == 0)
            {
                TypePersonDto.Id = id;
            }

            if (TypePersonDto.Id != id)
            {
                return BadRequest();
            }

            if (TypePersonDto == null)
            {
                return NotFound();
            }

            var TypePerson = _mapper.Map<TypePerson>(TypePersonDto);
            _unitOfWork.TypePersons.Update(TypePerson);
            await _unitOfWork.SaveAsync();
            return TypePersonDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var TypePerson = await _unitOfWork.TypePersons.GetByIdAsync(id);

            if (TypePerson == null)
            {
                return NotFound();
            }

            _unitOfWork.TypePersons.Remove(TypePerson);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}