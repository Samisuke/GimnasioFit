using Microsoft.AspNetCore.Mvc;
using Mapster;
using GimnasioFit.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using GimnasioFit.Core.Services;
using FluentValidation;
using System.Security.Claims;

namespace GimnasioFit.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClaseController : ControllerBase
    {
        private readonly IClaseService _service;
        private readonly IValidator<ClaseCreateDto> _validator;
        public ClaseController(IClaseService service, IValidator<ClaseCreateDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaseReadDto>>> GetClase()
        {
            var resultado = await _service.GetTodasLasClasesAsync();
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);

            return Ok(resultado.Valor.Adapt<IEnumerable<ClaseReadDto>>());
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<IEnumerable<ClaseReadDto>>> GetClaseNombre([FromRoute(Name = "nombre")]string nombreClase)
        {
            var resultado = await _service.GetTodasLasClasesDeUnNombreAsync(nombreClase);
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);
            
            return Ok(resultado.Valor.Adapt<IEnumerable<ClaseReadDto>>());
        }

        [HttpPost]
        [Authorize(policy: "NivelProfesor")]
        public async Task<ActionResult> PostClase([FromBody] ClaseCreateDto claseCreateDto)
        {
            var validationResult = await _validator.ValidateAsync(claseCreateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    Campo = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }
            
            var profesorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (profesorIdClaim is null || !int.TryParse(profesorIdClaim, out int profesorId)) return Unauthorized();
            
            var resultado = await _service.PostUnaClaseAsync(
                claseCreateDto.Nombre,
                claseCreateDto.Descripcion,
                claseCreateDto.CapacidadMaxima,
                profesorId);
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);

            return Ok(resultado.Valor.Adapt<ClaseReadDto>());
        }

        [HttpPatch("{id}")]
        [Authorize(policy: "NivelProfesor")]
        public async Task<ActionResult> PatchClase(int id, [FromBody] ClasePatchDto clasePatchDto)
        {
            var resultado = await _service.PatchUnaClasePorIdAsync(
                id,
                clasePatchDto.Nombre,
                clasePatchDto.Descripcion,
                clasePatchDto.CapacidadMaxima,
                clasePatchDto.Horario,
                clasePatchDto.ProfesorId);
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);

            return Ok(resultado.Valor.Adapt<ClaseReadDto>());
        }
    }
}