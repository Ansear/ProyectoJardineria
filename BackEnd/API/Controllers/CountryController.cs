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
[ApiVersion("1.0")]
[ApiVersion("1.1")]
public class CountryController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CountryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<CountryDto>>> Get()
    {
        var countries = await _unitOfWork.Countries.GetAllAsync();
        return _mapper.Map<List<CountryDto>>(countries);
    }
    [HttpGet]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<StateDto>>> Get2()
    {
        var countries = await _unitOfWork.States.GetAllAsync();
        return _mapper.Map<List<StateDto>>(countries);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CountryDto>> Get(int id)
    {
        var countries = await _unitOfWork.Countries.GetByIdAsync(id);
        if (countries == null)
            return NotFound();
        return _mapper.Map<CountryDto>(countries);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CountryDto>> Post([FromBody] CountryDto countryDto)
    {
        var countries = _mapper.Map<Country>(countryDto);
        _unitOfWork.Countries.Add(countries);
        await _unitOfWork.SaveAsync();
        if (countries == null)
            return BadRequest();
        countryDto.Id = countries.Id;
        return CreatedAtAction(nameof(Post), new { Id = countryDto.Id }, countryDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CountryDto>> Put(int id, [FromBody] CountryDto countryDto)
    {
        if (countryDto == null)
            return BadRequest();
        if (countryDto.Id == 0)
            countryDto.Id = id;
        if (countryDto.Id != id)
            return NotFound();
        var countries = _mapper.Map<Country>(countryDto);
        _unitOfWork.Countries.Update(countries);
        await _unitOfWork.SaveAsync();
        return countryDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var countries = await _unitOfWork.Countries.GetByIdAsync(id);
        if (countries == null)
            return NotFound();
        _unitOfWork.Countries.Remove(countries);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}