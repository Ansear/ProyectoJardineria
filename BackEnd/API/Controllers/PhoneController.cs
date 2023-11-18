using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class PhoneController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PhoneController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PhoneDto>>> Get()
    {
        var result = await _unitOfWork.Phones.GetAllAsync();
        return _mapper.Map<List<PhoneDto>>(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PhoneDto>> Get(int id)
    {
        var result = await _unitOfWork.Phones.GetByIdAsync(id);
        if (result == null)
            return NotFound();
        return _mapper.Map<PhoneDto>(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PhoneDto>> Post([FromBody] PhoneDto phoneDto)
    {
        var result = _mapper.Map<Phone>(phoneDto);
        _unitOfWork.Phones.Add(result);
        await _unitOfWork.SaveAsync();
        if (result == null)
            return BadRequest();
        phoneDto.Id = result.Id;
        return CreatedAtAction(nameof(Post), new { Id = phoneDto.Id }, phoneDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PhoneDto>> Put(int id, [FromBody] PhoneDto phoneDto)
    {
        if (phoneDto == null)
            return BadRequest();
        if (phoneDto.Id == 0)
            phoneDto.Id = id;
        if (phoneDto.Id != id)
            return NotFound();
        var result = _mapper.Map<Phone>(phoneDto);
        _unitOfWork.Phones.Update(result);
        await _unitOfWork.SaveAsync();
        return phoneDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _unitOfWork.Phones.GetByIdAsync(id);
        if (result == null)
            return NotFound();
        _unitOfWork.Phones.Remove(result);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}