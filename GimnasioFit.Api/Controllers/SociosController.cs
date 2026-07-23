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

    public class SociosController : ControllerBase
    {
        private readonly ISocioService _service;
        private readonly IValidator<SocioCreateDto> _validator;

        public SociosController(ISocioService service, IValidator<SocioCreateDto> validator)
        {
            _service = service;
            _validator = validator;
        }
   

        [HttpGet]
        [Authorize(policy: "NivelProfesor")]
        public async Task<ActionResult<IEnumerable<SocioReadDto>>> Getsocios()
        {
            var resultado = await _service.GetSociosAsync();
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);

            return Ok(resultado.Valor.Adapt<IEnumerable<SocioReadDto>>());
        }

        [HttpGet("{id}")]
        [Authorize(policy: "NivelProfesor")]
        public async Task<ActionResult> GetSocioPorId(int id)
        {
            var resultado = await _service.GetSocioPorIdAsync(id);
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);

            return Ok(resultado.Valor.Adapt<SocioReadDto>());
        }

        [HttpPost]
        [Authorize(policy: "NivelProfesor")]
        public async Task<ActionResult> PostSocio([FromBody] SocioCreateDto socioCreateDto)
        {
            var validationResult = await _validator.ValidateAsync(socioCreateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    Campo = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }
        
            var resultado = await _service.PostSocioAsync(
                socioCreateDto.Nombre,
                socioCreateDto.Pass,
                socioCreateDto.Email,
                socioCreateDto.TarifaPremium
            );

            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);
            
            var socioDto = resultado.Valor.Adapt<SocioReadDto>();
            return CreatedAtAction(nameof(GetSocioPorId), new {id = socioDto.Id}, socioDto);
        }

        [HttpPatch("{id}")]
        [Authorize(policy: "NivelProfesor")]
        public async Task<ActionResult> PatchSocio(int id, [FromBody] SocioPatchDto socioPatchDto)
        {
            var resultado = await _service.PatchSocioAsync(
                id,
                socioPatchDto.Nombre,
                socioPatchDto.Email,
                socioPatchDto.TarifaPremium
            );
            if (!resultado.EsCorrecto || resultado.Valor is null) return BadRequest(resultado.MensajeError);            

            return Ok(resultado.Valor.Adapt<SocioReadDto>());
        }
    }
}