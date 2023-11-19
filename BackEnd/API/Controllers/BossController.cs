using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data;

namespace API.Controllers
{
    public class BossController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardenContext _context;

        public BossController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BossDto>>> Get()
        {
            var bosses = await _unitOfWork.Bosses.GetAllAsync();
            return _mapper.Map<List<BossDto>>(bosses);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BossDto>> Get(string id)
        {
            var bosses = await _unitOfWork.Bosses.GetByIdAsync(id);

            if (bosses == null)
            {
                return NotFound();
            }

            return _mapper.Map<BossDto>(bosses);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Boss>> Post(BossDto bossDto)
        {
            var boss = _mapper.Map<Boss>(bossDto);
            this._unitOfWork.Bosses.Add(boss);
            await _unitOfWork.SaveAsync();

            if (boss == null)
            {
                return BadRequest();
            }
            bossDto.Id = boss.Id;
            return CreatedAtAction(nameof(Post), new { id = bossDto.Id }, bossDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BossDto>> Put(string id, [FromBody] BossDto bossDto)
        {
            if (bossDto.Id == null)
            {
                bossDto.Id = id;
            }

            if (bossDto.Id != id)
            {
                return BadRequest();
            }

            if (bossDto == null)
            {
                return NotFound();
            }

            var boss = _mapper.Map<Boss>(bossDto);
            _unitOfWork.Bosses.Update(boss);
            await _unitOfWork.SaveAsync();
            return bossDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var bosses = await _unitOfWork.Bosses.GetByIdAsync(id);

            if (bosses == null)
            {
                return NotFound();
            }

            _unitOfWork.Bosses.Remove(bosses);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}