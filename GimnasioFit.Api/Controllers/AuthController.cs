using Microsoft.AspNetCore.Mvc;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Api.Dtos;
using GimnasioFit.Core.Services;
using BCrypt.Net;

namespace GimnasioFit.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IEmpleadoRepository _empleadoRepo;
        private readonly ISocioRepository _socioRepo;
        private readonly ITokenService _tokenService;
        public AuthController(IEmpleadoRepository empleadoRepo, ISocioRepository socioRepo, ITokenService tokenService)
        {
            _empleadoRepo = empleadoRepo;
            _socioRepo = socioRepo;
            _tokenService = tokenService;
        }    

        [HttpPost("login/empleado")]
        public async Task<ActionResult> LoginEmpleado([FromBody] LoginDto loginDto)
        {
            var empleado = await _empleadoRepo.ObtenerEmpleadoPorEmailAsync(loginDto.Email);

            if (empleado is null || !BCrypt.Net.BCrypt.Verify(loginDto.Pass, empleado.Pass))
            {
                return Unauthorized("Credenciales incorrectas. Intentalo de nuevo");
            }

            var token = _tokenService.GenerarToken(empleado.Id, empleado.Nombre, empleado.Email, "Empleado", empleado.NivelAcceso.ToString());
            return Ok(new { Token = token, Mensaje = "Login correcto" }); // El token lo devuelvo para las pruebas, eliminar en la app final.
        }

        [HttpPost("login/socio")]
        public async Task<ActionResult> LoginSocio([FromBody] LoginDto loginDto)
        {
            var socio = await _socioRepo.ObtenerSocioPorEmailAsync(loginDto.Email);

            if (socio is null || !BCrypt.Net.BCrypt.Verify(loginDto.Pass, socio.Pass))
            {
                return Unauthorized("Credenciales incorrectas. Intentalo de nuevo");
            }
            
            var token = _tokenService.GenerarToken(socio.Id, socio.Nombre, socio.Email, "Socio");
            return Ok(new { Token = token, Mensaje = "Login correcto" }); // El token lo devuelvo para las pruebas, eliminar en la app final.
        }


    }
}