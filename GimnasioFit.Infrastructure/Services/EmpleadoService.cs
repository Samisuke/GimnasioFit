using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Core.Services;
using GimnasioFit.Core.Common;
using System.IO.Pipelines;

namespace GimnasioFit.Infrastructure.Services
{
    public class EmpleadoService: IEmpleadoService
    {
        private readonly IEmpleadoRepository _repoEmpleado;
        public EmpleadoService(IEmpleadoRepository repoEmpleado)
        {
            _repoEmpleado = repoEmpleado;
        }
        public async Task<Resultado<IEnumerable<Empleado>>> GetTodosLosEmpleadosAsync()
        {
            var empleadosDb = await _repoEmpleado.ObtenerTodosAsync();
            if (!empleadosDb.Any()) return Resultado<IEnumerable<Empleado>>.Mal("ERROR. Lista de empleados vacia.");

            return Resultado<IEnumerable<Empleado>>.Bien(empleadosDb);
        }
        public async Task<Resultado<Empleado>> GetUnEmpleadoPorId(int id) 
        {
            var empleadoDb = await _repoEmpleado.ObtenerPorIdAsync(id);
            if (empleadoDb is null) return Resultado<Empleado>.Mal("ERROR. No se encuentra el empleado.");

            return Resultado<Empleado>.Bien(empleadoDb);
        }
        public async Task<Resultado<Empleado>> PostEmpleado(
            string nombreEmpleado,
            string passEmpleado,
            string emailEmpleado,
            string puestoEmpleado,
            int nivelAccesoEmpleado
        )
        {
            var emailRepetido = await _repoEmpleado.ObtenerEmpleadoPorEmailAsync(emailEmpleado);
            if (emailRepetido is not null && emailRepetido.Email == emailEmpleado) return Resultado<Empleado>.Mal("ERROR. El email ya existe.");

            var nuevoEmpleado = new Empleado
            {
                Nombre = nombreEmpleado,
                Pass = BCrypt.Net.BCrypt.HashPassword(passEmpleado),
                Email = emailEmpleado,
                Puesto = puestoEmpleado,
                FechaContratacion = DateTime.UtcNow,
                NivelAcceso = nivelAccesoEmpleado
            };

            await _repoEmpleado.CrearEmpleadoAsync(nuevoEmpleado);
            var guardadoExitoso = await _repoEmpleado.GuardarCambiosAsync();
            if (!guardadoExitoso) return Resultado<Empleado>.Mal("ERROR. No se ha podido guardar el empleado.");

            return Resultado<Empleado>.Bien(nuevoEmpleado);
             
        }
        public async Task<Resultado<Empleado>> PatchEmpleado(
            int id,
            string? nombreEmpleado,
            string? emailEmpleado,
            string? puestoEmpleado,
            int? nivelAccesoEmpleado
        )
        {
            int numeroCambios = 0;
            
            var empleadoDb = await _repoEmpleado.ObtenerPorIdAsync(id);
            if (empleadoDb is null) return Resultado<Empleado>.Mal("ERROR. No se encuentra el empleado.");

            if (nombreEmpleado is not null)
            {
                empleadoDb.Nombre = nombreEmpleado;
                numeroCambios += 1;
            }

            if (emailEmpleado is not null)
            {
                var emailRepetido = await _repoEmpleado.ObtenerEmpleadoPorEmailAsync(emailEmpleado);
                if (emailRepetido is not null && emailRepetido.Email == emailEmpleado) return Resultado<Empleado>.Mal("ERROR. El email ya existe.");

                empleadoDb.Email = emailEmpleado;
                numeroCambios += 1;
            }

            if (puestoEmpleado is not null)
            {
                empleadoDb.Puesto = puestoEmpleado;
                numeroCambios += 1;
            }

            if (nivelAccesoEmpleado.HasValue)
            {
                empleadoDb.NivelAcceso = nivelAccesoEmpleado.Value;
                numeroCambios += 1;
            }

            if (numeroCambios == 0) return Resultado<Empleado>.Mal("No se han hecho cambios.");
            var guardadoExitoso = await _repoEmpleado.GuardarCambiosAsync();
            if (!guardadoExitoso) return Resultado<Empleado>.Mal("ERROR. No se han podido guardar los cambios");

            return Resultado<Empleado>.Bien(empleadoDb);
        }
    }
}