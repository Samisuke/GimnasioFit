namespace GimnasioFit.Core.Models
{
    public class Reserva
    {
        public int Id {get; set;}
        public DateTime FechaReserva {get; set;}

        public int SocioId {get; set;}
        public Socio Socio {get; set;} = null!;
        
        public int ClaseId {get; set;}
        public Clase Clase {get; set;} = null!;
    }
}