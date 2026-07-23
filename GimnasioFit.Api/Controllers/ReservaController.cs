using Microsoft.AspNetCore.Mvc;
using GimnasioFit.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using System.Security.Claims;
using GimnasioFit.Core.Services;
using Mapster;

namespace GimnasioFit.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _service;
        private readonly IValidator<ReservaCreateDto> _validator;
        public ReservaController(IReservaService service, IValidator<ReservaCreateDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpGet("clase/{claseId}")]
        [Authorize(policy: "NivelProfesor")]
        public async Task<ActionResult<IEnumerable<ReservaReadDto>>> GetReservasDeUnaClase(int claseId)
        {
            var resultado = await _service.GetReservasDeUnaClaseAsync(claseId);
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);

            return Ok(resultado.Valor.Adapt<IEnumerable<ReservaReadDto>>());
        }

        [HttpGet("mis-reservas")]
        [Authorize(Roles = "Socio")]
        public async Task<ActionResult<IEnumerable<ReservaReadDto>>> GetMisReservas()
        {
            var socioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (socioIdClaim is null || !int.TryParse(socioIdClaim, out int socioId)) return Unauthorized();

            var resultado = await _service.GetReservasDeUnSocioAsync(socioId);
            if(!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);

            return Ok(resultado.Valor.Adapt<IEnumerable<ReservaReadDto>>());
        }

        [HttpPost]
        [Authorize(Roles = "Socio")]
        public async Task<ActionResult> PostReserva([FromBody] ReservaCreateDto reservaCreateDto)
        {
            var validationResult = await _validator.ValidateAsync(reservaCreateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    Campo = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            var socioEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (socioEmail is null) return Unauthorized();

            var resultado = await _service.PostReservaAsync(socioEmail, reservaCreateDto.ClaseId);
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);

            return Ok(resultado.Valor.Adapt<ReservaReadDto>());
        }      
    } 
}