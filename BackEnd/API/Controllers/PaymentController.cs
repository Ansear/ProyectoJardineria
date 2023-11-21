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
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace API.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GardenContext _context;

        public PaymentController(IUnitOfWork unitOfWork, IMapper mapper, GardenContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> Get()
        {
            var nombreVariable = await _unitOfWork.Payments.GetAllAsync();
            return _mapper.Map<List<PaymentDto>>(nombreVariable);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentDto>> Get(int id)
        {
            var nombreVariable = await _unitOfWork.Payments.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            return _mapper.Map<PaymentDto>(nombreVariable);
        }

        [HttpGet("DistinctPaymentForms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PaymentForm>>> GetDistinctPaymentForms()
        {
            try
            {
                var distinctPaymentForms = await _context.Payments
                    .Select(payment => payment.PaymentForm)
                    .Distinct()
                    .ToListAsync();

                if (distinctPaymentForms.Any())
                {
                    return Ok(distinctPaymentForms);
                }
                else
                {
                    return BadRequest("No se encontraron formas de pago distintas en la tabla Payment.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Payment>> Post(PaymentDto PaymentDto)
        {
            var nombreVariable = _mapper.Map<Payment>(PaymentDto);
            this._unitOfWork.Payments.Add(nombreVariable);
            await _unitOfWork.SaveAsync();

            if (nombreVariable == null)
            {
                return BadRequest();
            }
            PaymentDto.Id = nombreVariable.Id;
            return CreatedAtAction(nameof(Post), new { id = PaymentDto.Id }, PaymentDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentDto>> Put(int id, [FromBody] PaymentDto PaymentDto)
        {
            if (PaymentDto.Id == 0)
            {
                PaymentDto.Id = id;
            }

            if (PaymentDto.Id != id)
            {
                return BadRequest();
            }

            if (PaymentDto == null)
            {
                return NotFound();
            }

            // Por si requiero la fecha actual
            /*if (PaymentDto.Fecha == DateTime.MinValue)
            {
                PaymentDto.Fecha = DateTime.Now;
            }*/

            var nombreVariable = _mapper.Map<Payment>(PaymentDto);
            _unitOfWork.Payments.Update(nombreVariable);
            await _unitOfWork.SaveAsync();
            return PaymentDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var nombreVariable = await _unitOfWork.Payments.GetByIdAsync(id);

            if (nombreVariable == null)
            {
                return NotFound();
            }

            _unitOfWork.Payments.Remove(nombreVariable);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}