using Microsoft.AspNetCore.Mvc;
using GimnasioFit.Core.Models;
using Mapster;
using GimnasioFit.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using GimnasioFit.Core.Services;


namespace GimnasioFit.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly IEmpleadoService _service;
        private readonly IValidator<EmpleadoCreateDto> _validator;
        public EmpleadosController(IEmpleadoService service, IValidator<EmpleadoCreateDto> validator)
        {
            _service = service;
            _validator = validator;
        }


        [HttpGet]
        [Authorize(policy: "NivelGestor")]
        public async Task<ActionResult<IEnumerable<SocioReadDto>>> GetEmpleados()
        {
            var resultado = await _service.GetTodosLosEmpleadosAsync();
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);

            return Ok(resultado.Valor.Adapt<IEnumerable<EmpleadoReadDto>>());
        }

        [HttpGet("{id}")]
        [Authorize(policy: "NivelGestor")]
        public async Task<ActionResult> GetEmpleado(int id)
        {
            var resultado = await _service.GetUnEmpleadoPorId(id);
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);

            return Ok(resultado.Valor.Adapt<EmpleadoReadDto>());
        }

        [HttpPost]
        [Authorize(policy: "NivelGestor")]
        public async Task<ActionResult> PostEmpleado([FromBody] EmpleadoCreateDto empleadoCreateDto)
        {
            var validationResult = await _validator.ValidateAsync(empleadoCreateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    Campo = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            var resultado = await _service.PostEmpleado(
                empleadoCreateDto.Nombre,
                empleadoCreateDto.Pass,
                empleadoCreateDto.Email,
                empleadoCreateDto.Puesto,
                empleadoCreateDto.NivelAcceso
            );
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);
            
            var empleadoDto = resultado.Valor.Adapt<EmpleadoReadDto>();
            return CreatedAtAction(nameof(GetEmpleado), new {id = empleadoDto.Id}, empleadoDto);
        }

        [HttpPatch("{id}")]
        [Authorize(policy: "NivelAdmin")]
        public async Task<ActionResult> PatchEmpleado(int id, [FromBody] EmpleadoPatchDto empleadoPatchDto)
        {
            var resultado = await _service.PatchEmpleado(
                id,
                empleadoPatchDto.Nombre,
                empleadoPatchDto.Email,
                empleadoPatchDto.Puesto,
                empleadoPatchDto.NivelAcceso

            );
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);

            return Ok(resultado.Valor.Adapt<EmpleadoReadDto>());
        }
    }
}
