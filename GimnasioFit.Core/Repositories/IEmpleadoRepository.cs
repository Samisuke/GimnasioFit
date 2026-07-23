using GimnasioFit.Core.Models;

namespace GimnasioFit.Core.Repositories
{
    public interface IEmpleadoRepository
    {
        Task<IEnumerable<Empleado>> ObtenerTodosAsync();
        Task<Empleado?> ObtenerPorIdAsync(int id);
        Task CrearEmpleadoAsync(Empleado empleado);
        Task<bool> GuardarCambiosAsync();
        Task<Empleado?> ObtenerEmpleadoPorEmailAsync(string correo);
    }
}