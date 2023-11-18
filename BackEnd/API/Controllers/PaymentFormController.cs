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

namespace API.Controllers
{
    public class PaymentFormController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentFormController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PaymentFormDto>>> Get()
        {
            var nombreVariable = await _unitOfWork.PaymentForms.GetAllAsync();
            return _mapper.Map<List<PaymentFormDto>>(nombreVariable);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentFormDto>> Get(string id)
        {
            var nombreVariable = await _unitOfWork.PaymentForms.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            return _mapper.Map<PaymentFormDto>(nombreVariable);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentForm>> Post(PaymentFormDto PaymentFormDto)
        {
            var nombreVariable = _mapper.Map<PaymentForm>(PaymentFormDto);
            this._unitOfWork.PaymentForms.Add(nombreVariable);
            await _unitOfWork.SaveAsync();

            if (nombreVariable == null)
            {
                return BadRequest();
            }
            PaymentFormDto.Id = nombreVariable.Id;
            return CreatedAtAction(nameof(Post), new { id = PaymentFormDto.Id }, PaymentFormDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentFormDto>> Put(string id, [FromBody] PaymentFormDto PaymentFormDto)
        {
            if (PaymentFormDto.Id == "" || PaymentFormDto.Id == null)
            {
                PaymentFormDto.Id = id;
            }

            if(PaymentFormDto.Id != id)
            {
                return BadRequest();
            }

            if(PaymentFormDto == null)
            {
                return NotFound();
            }

            var nombreVariable = _mapper.Map<PaymentForm>(PaymentFormDto);
            _unitOfWork.PaymentForms.Update(nombreVariable);
            await _unitOfWork.SaveAsync();
            return PaymentFormDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var nombreVariable = await _unitOfWork.PaymentForms.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            _unitOfWork.PaymentForms.Remove(nombreVariable);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}