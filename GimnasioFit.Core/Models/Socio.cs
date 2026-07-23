namespace GimnasioFit.Core.Models
{
    public class Socio
    {
        public int Id {get; set;}
        public string Nombre {get; set;} = string.Empty;
        public string Pass {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public DateTime FechaAlta {get; set;}
        public bool TarifaPremium {get; set;}

        public List<Reserva> ReservasActivas {get; set;} = [];
    }    
}