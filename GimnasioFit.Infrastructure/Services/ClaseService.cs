using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Core.Services;
using GimnasioFit.Core.Common;

namespace GimnasioFit.Infrastructure.Services
{
    public class ClaseService : IClaseService
    {
        private readonly IClaseRepository _repoClase;

        public ClaseService(IClaseRepository repoClase)
        {
            _repoClase = repoClase;
        }

        public async Task<Resultado<IEnumerable<Clase>>>  GetTodasLasClasesAsync()
        {
            var clasesDb = await _repoClase.ObtenerTodasLasClasesAsync();
            if (!clasesDb.Any()) return Resultado<IEnumerable<Clase>>.Mal("ERROR. No hay clases disponibles.");

            return Resultado<IEnumerable<Clase>>.Bien(clasesDb);
        }

        public async Task<Resultado<IEnumerable<Clase>>> GetTodasLasClasesDeUnNombreAsync(string nombreClase)
        {
            var clasesDb = await _repoClase.ObtenerTodasLasClasesDeUnNombreAsync(nombreClase);
            if (!clasesDb.Any()) return Resultado<IEnumerable<Clase>>.Mal("ERROR. No hay clases disponibles.");

            return Resultado<IEnumerable<Clase>>.Bien(clasesDb);
        }

        public async Task<Resultado<Clase>> PostUnaClaseAsync(string nombreClase, 
            string descripcionClase,
            int capacidadMaximaClase,
            int profesorId)
        {
            var claseNueva = new Clase
            {
                Nombre = nombreClase,
                Descripcion = descripcionClase,
                CapacidadMaxima = capacidadMaximaClase,
                Horario = DateTime.UtcNow,
                ProfesorId = profesorId
            };

            await _repoClase.CrearUnaClaseNuevaAsync(claseNueva);
            var guardadoExitoso = await _repoClase.GuardarCambiosAsync();
            if (!guardadoExitoso) return Resultado<Clase>.Mal("ERROR. No se ha podido guardar la clase.");

            return Resultado<Clase>.Bien(claseNueva);
        }

        public async Task<Resultado<Clase>> PatchUnaClasePorIdAsync(int idClase,
            string? nombreClase,
            string? descripcionClase,
            int? capacidadMaximaClase,
            DateTime? dateTimeClase,
            int? profesorIdClase)
        {   
            int numeroCambios = 0;

            var claseDb = await _repoClase.ObtenerUnaClasePorIdAsync(idClase);
            if (claseDb is null) return Resultado<Clase>.Mal("ERROR. No se ha podido guardar la clase.");

            if(nombreClase is not null)
            {
                claseDb.Nombre = nombreClase;
                numeroCambios += 1;
            }
            if (descripcionClase is not null)
            {
                claseDb.Descripcion = descripcionClase;
                numeroCambios += 1;
            }
            if (capacidadMaximaClase.HasValue)
            {
                claseDb.CapacidadMaxima = capacidadMaximaClase.Value;
                numeroCambios += 1;
            }
            if (dateTimeClase.HasValue)
            {
                claseDb.Horario = dateTimeClase.Value;
                numeroCambios += 1;
            }
            if (profesorIdClase.HasValue)
            {
                claseDb.ProfesorId = profesorIdClase.Value;
                numeroCambios += 1;
            }

            if (numeroCambios == 0) return Resultado<Clase>.Mal("No habia cambios para realizar.");

            var guardadoExitoso = await _repoClase.GuardarCambiosAsync();
            if (!guardadoExitoso) return Resultado<Clase>.Mal("ERROR. No se han podido guardar los cambios.");

            return Resultado<Clase>.Bien(claseDb);
        }
    }
}
