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
public class OfficeController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OfficeController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<OfficeDto>>> Get()
    {
        var result = await _unitOfWork.Offices.GetAllAsync();
        return _mapper.Map<List<OfficeDto>>(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OfficeDto>> Get(string id)
    {
        var offices = await _unitOfWork.Offices.GetByIdAsync(id);
        if (offices == null)
            return NotFound();
        return _mapper.Map<OfficeDto>(offices);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OfficeDto>> Post([FromBody] OfficeDto officeDto)
    {
        var result = _mapper.Map<Office>(officeDto);
        _unitOfWork.Offices.Add(result);
        await _unitOfWork.SaveAsync();
        if (result == null)
            return BadRequest();
        officeDto.Id = result.Id;
        return CreatedAtAction(nameof(Post), new { Id = officeDto.Id }, officeDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OfficeDto>> Put(string id, [FromBody] OfficeDto officeDto)
    {
        if (officeDto == null)
            return BadRequest();
        if (officeDto.Id == "")
            officeDto.Id = id;
        if (officeDto.Id != id)
            return NotFound();
        var result = _mapper.Map<Office>(officeDto);
        _unitOfWork.Offices.Update(result);
        await _unitOfWork.SaveAsync();
        return officeDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _unitOfWork.Offices.GetByIdAsync(id);
        if (result == null)
            return NotFound();
        _unitOfWork.Offices.Remove(result);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}