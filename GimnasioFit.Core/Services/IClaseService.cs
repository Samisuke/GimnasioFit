using GimnasioFit.Core.Models;
using GimnasioFit.Core.Common;

namespace GimnasioFit.Core.Services
{
    public interface IClaseService
    {
        Task<Resultado<IEnumerable<Clase>>> GetTodasLasClasesAsync();
        Task<Resultado<IEnumerable<Clase>>> GetTodasLasClasesDeUnNombreAsync(string nombreClase);
        Task<Resultado<Clase>> PostUnaClaseAsync(string nombreClase, 
            string descripcionClase,
            int capacidadMaxima,
            int profesorId);
        Task<Resultado<Clase>> PatchUnaClasePorIdAsync(int idClase,
            string? nombreClase,
            string? descripcionClase,
            int? capacidadMaximaClase,
            DateTime? dateTimeClase,
            int? profesorIdClase);
    }
}