using Mapster;
using GimnasioFit.Core.Models;
using GimnasioFit.Api.Dtos;

namespace GimnasioFit.Api.Config
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<Clase, ClaseReadDto>
                .NewConfig()
                .Map(dest => dest.Profesor.ProfesorNombre, src => src.Profesor != null ? src.Profesor.Nombre : string.Empty);

            TypeAdapterConfig<Reserva, ReservaReadDto>
                .NewConfig()
                .Map(dest => dest.Socio.NombreSocio, src => src.Socio != null ? src.Socio.Nombre : string.Empty)
                .Map(dest => dest.Clase.NombreClase, src => src.Clase != null ? src.Clase.Nombre : string.Empty)
                .Map(dest => dest.Clase.NombreProfesor, src => src.Clase != null && src.Clase.Profesor != null ? src.Clase.Profesor.Nombre : string.Empty);
        }   
    }
}