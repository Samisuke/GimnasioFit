namespace GimnasioFit.Core.Models
{
    public class Empleado
    {
        public int Id {get; set;}
        public string Nombre {get; set;} = string.Empty;
        public string Pass {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public string Puesto {get; set;} = string.Empty;
        public DateTime FechaContratacion {get; set;}
        public int NivelAcceso {get; set;}

        public List<Clase> ClasesProfesor {get; set;} = [];
    }
}