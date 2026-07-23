using GimnasioFit.Core.Models;

namespace GimnasioFit.Core.Repositories
{
    public interface IClaseRepository
    {
        Task<IEnumerable<Clase>> ObtenerTodasLasClasesAsync();
        Task<IEnumerable<Clase>> ObtenerTodasLasClasesDeUnNombreAsync(string nombreClase);
        Task CrearUnaClaseNuevaAsync(Clase clase);
        Task<bool> GuardarCambiosAsync();


        Task<Clase?> ObtenerUnaClasePorIdAsync(int id);
    }
}