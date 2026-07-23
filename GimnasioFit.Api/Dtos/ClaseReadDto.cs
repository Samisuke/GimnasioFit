namespace GimnasioFit.Api.Dtos
{
    public class ClaseReadDto
    {
        public int Id {get; set;}
        public string Nombre {get; set;} = string.Empty;
        public string Descripcion {get; set;} = string.Empty;
        public int CapacidadMaxima {get; set;}
        public DateTime Horario {get; set;}
        public EmpleadoReadClaseDto Profesor {get; set;} = null!;
    }

    public class EmpleadoReadClaseDto
    {
        public string ProfesorNombre {get; set;} = string.Empty;
    }
}