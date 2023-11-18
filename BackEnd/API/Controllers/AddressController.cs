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

namespace API.Controllers;
public class AddressController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddressController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AddressDto>>> Get()
    {
        var address = await _unitOfWork.Address.GetAllAsync();
        return _mapper.Map<List<AddressDto>>(address);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddressDto>> Get(int id)
    {
        var address = await _unitOfWork.Address.GetByIdAsync(id);
        if (address == null)
            return NotFound();
        return _mapper.Map<AddressDto>(address);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AddressDto>> Post([FromBody] AddressDto addressDto)
    {
        var address = _mapper.Map<Address>(addressDto);
        _unitOfWork.Address.Add(address);
        await _unitOfWork.SaveAsync();
        if (address == null)
            return BadRequest();
        addressDto.Id = address.Id;
        return CreatedAtAction(nameof(Post), new { Id = addressDto.Id }, addressDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddressDto>> Put(int id, [FromBody] AddressDto addressDto)
    {
        if (addressDto == null)
            return BadRequest();
        if (addressDto.Id == 0)
            addressDto.Id = id;
        if (addressDto.Id != id)
            return NotFound();
        var rol = _mapper.Map<Address>(addressDto);
        _unitOfWork.Address.Update(rol);
        await _unitOfWork.SaveAsync();
        return addressDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var address = await _unitOfWork.Address.GetByIdAsync(id);
        if (address == null)
            return NotFound();
        _unitOfWork.Address.Remove(address);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }
}