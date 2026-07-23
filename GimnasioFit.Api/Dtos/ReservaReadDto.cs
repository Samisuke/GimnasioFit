namespace GimnasioFit.Api.Dtos
{
    public class ReservaReadDto
    {
        public DateTime FechaReserva {get; set;}

        public SocioReadReservaDto Socio {get; set;} = null!;
        
        public ClaseReadReservaDto Clase {get; set;} = null!;
    }

    public class SocioReadReservaDto
    {
        public string NombreSocio {get; set;} = string.Empty;
    }

    public class ClaseReadReservaDto
    {
        public string NombreClase {get; set;} = string.Empty;
        public string NombreProfesor {get; set;} = string.Empty;
    }
}