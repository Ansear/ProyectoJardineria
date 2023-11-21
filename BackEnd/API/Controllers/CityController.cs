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
public class CityController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly GardenContext _context;

    public CityController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _context = context;
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

    // Devuelve un listado con la ciudad y el teléfono de las oficinas de España.
    [HttpGet("CityAndPhoneFromOffices/{country}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<IQueryable<CityAndPhoneFromSpain>> GetCityAndPhoneFromSpain(string country)
    {
        var result = _context.Cities
            .Where(x => x.States.Countries.Name == country)
            .SelectMany(x => x.Address)
            .SelectMany(address => address.Offices)
            .Select(office => new CityAndPhoneFromSpain
            {
                CityName = office.Address.Cities.Name,
                Phone = office.Phones.PhoneNumber
            });

        return Task.FromResult(result);
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

    [HttpGet("CustomerCountByCityStartingWithM")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetCustomerCountByCityStartingWithM()
    {
        var customerCountByCity = _context.Cities
            .Where(c => c.Name.StartsWith("M"))
            .GroupJoin(
                _context.Address,
                city => city.Id,
                address => address.IdCity,
                (city, addresses) => new { City = city, Addresses = addresses }
            )
            .SelectMany(
                x => x.Addresses.DefaultIfEmpty(),
                (cityGroup, address) => new { City = cityGroup.City, Address = address }
            )
            .GroupJoin(
                _context.Customers,
                x => x.Address.Id,
                customer => customer.AddressId,
                (x, customers) => new { City = x.City, Customers = customers }
            )
            .Select(x => new 
            {
                CityName = x.City.Name,
                CustomerCount = x.Customers.Count()
            })
            .ToList();

        if (customerCountByCity == null || !customerCountByCity.Any())
        {
            return NotFound("No customer counts by city starting with 'M' found.");
        }

        return Ok(customerCountByCity);
    }
}