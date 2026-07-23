namespace GimnasioFit.Core.Models
{
    public class Clase
    {
        public int Id {get; set;}
        public string Nombre {get; set;} = string.Empty;
        public string Descripcion {get; set;} = string.Empty;
        public int CapacidadMaxima {get; set;}
        public DateTime Horario {get; set;}
        public int ProfesorId {get; set;}
        public Empleado Profesor {get; set;} = null!;
    }
}