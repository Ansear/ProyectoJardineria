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
public class CityController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CityController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<CityDto>>> Get()
    {
        var result = await _unitOfWork.Cities.GetAllAsync();
        return _mapper.Map<List<CityDto>>(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CityDto>> Get(int id)
    {
        var result = await _unitOfWork.Cities.GetByIdAsync(id);
        if (result == null)
            return NotFound();
        return _mapper.Map<CityDto>(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CityDto>> Post([FromBody] CityDto cityDto)
    {
        var result = _mapper.Map<City>(cityDto);
        _unitOfWork.Cities.Add(result);
        await _unitOfWork.SaveAsync();
        if (result == null)
            return BadRequest();
        cityDto.Id = result.Id;
        return CreatedAtAction(nameof(Post), new { Id = cityDto.Id }, cityDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CityDto>> Put(int id, [FromBody] CityDto cityDto)
    {
        if (cityDto == null)
            return BadRequest();
        if (cityDto.Id == 0)
            cityDto.Id = id;
        if (cityDto.Id != id)
            return NotFound();
        var result = _mapper.Map<City>(cityDto);
        _unitOfWork.Cities.Update(result);
        await _unitOfWork.SaveAsync();
        return cityDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _unitOfWork.Cities.GetByIdAsync(id);
        if (result == null)
            return NotFound();
        _unitOfWork.Cities.Remove(result);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}