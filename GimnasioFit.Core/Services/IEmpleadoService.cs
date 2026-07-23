using GimnasioFit.Core.Models;
using GimnasioFit.Core.Common;

namespace GimnasioFit.Core.Services
{
    public interface IEmpleadoService
    {
        Task<Resultado<IEnumerable<Empleado>>> GetTodosLosEmpleadosAsync();
        Task<Resultado<Empleado>> GetUnEmpleadoPorId(int id);
        Task<Resultado<Empleado>> PostEmpleado(
            string nombreEmpleado,
            string passEmpleado,
            string emailEmpleado,
            string puestoEmpleado,
            int nivelAccesoEmpleado
        );
        Task<Resultado<Empleado>> PatchEmpleado(
            int id,
            string? nombreEmpleado,
            string? emailEmpleado,
            string? puestoEmpleado,
            int? nivelAccesoEmpleado
        );
    }
}