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
public class StateController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StateController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<StateDto>>> Get()
    {
        var states = await _unitOfWork.States.GetAllAsync();
        return _mapper.Map<List<StateDto>>(states);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StateDto>> Get(int id)
    {
        var result = await _unitOfWork.States.GetByIdAsync(id);
        if (result == null)
            return NotFound();
        return _mapper.Map<StateDto>(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StateDto>> Post([FromBody] StateDto stateDto)
    {
        var result = _mapper.Map<State>(stateDto);
        _unitOfWork.States.Add(result);
        await _unitOfWork.SaveAsync();
        if (result == null)
            return BadRequest();
        stateDto.Id = result.Id;
        return CreatedAtAction(nameof(Post), new { Id = stateDto.Id }, stateDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StateDto>> Put(int id, [FromBody] StateDto stateDto)
    {
        if (stateDto == null)
            return BadRequest();
        if (stateDto.Id == 0)
            stateDto.Id = id;
        if (stateDto.Id != id)
            return NotFound();
        var result = _mapper.Map<State>(stateDto);
        _unitOfWork.States.Update(result);
        await _unitOfWork.SaveAsync();
        return stateDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _unitOfWork.States.GetByIdAsync(id);
        if (result == null)
            return NotFound();
        _unitOfWork.States.Remove(result);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}