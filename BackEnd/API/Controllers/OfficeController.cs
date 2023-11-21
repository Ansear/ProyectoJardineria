using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace API.Controllers;
public class OfficeController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly GardenContext _context;

    public OfficeController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<OfficeDto>>> Get()
    {
        var result = await _unitOfWork.Offices.GetAllAsync();
        return _mapper.Map<List<OfficeDto>>(result);
    }

    [HttpGet("OfficeCity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetOfficeCity()
    {

        var result = await _context.Offices.Include(o => o.Address)
        .Where(o => o.Address != null && o.Address.Cities != null)
        .Select(office => new
        {
            CodeOfOffice = office.Id,
            City = office.Address.Cities.Name
        })
        .ToListAsync();
        return Ok(result);
    }

    [HttpGet("OfficesWithoutSalesRepresentativesForFruitProducts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Office>>> GetOfficesWithoutSalesRepresentativesForFruitProducts()
    {
        try
        {
            var officesWithoutSalesRepresentatives = await _context.Offices
                .Where(office =>
                    !office.OfficeEmployees.Any(oe =>
                        oe.Employee.EmployeePosition == "Sales Representative" &&
                        oe.Employee.OrderCustomerEmployees.Any(oce =>
                            oce.Order.OrderDetails.Any(od =>
                                od.Product.Gamma.TextDescription == "Frutales"
                            )
                        )
                    )
                )
                .ToListAsync();

            if (officesWithoutSalesRepresentatives.Any())
            {
                return Ok(officesWithoutSalesRepresentatives);
            }
            else
            {
                return BadRequest("No se encontraron oficinas sin representantes de ventas para productos de la gama Frutales.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
        }
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