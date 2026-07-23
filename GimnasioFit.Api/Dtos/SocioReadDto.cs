namespace GimnasioFit.Api.Dtos
{
    public class SocioReadDto
    {
        public int Id {get; set;}
        public string Nombre {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public DateTime FechaAlta {get; set;}
        public bool TarifaPremium {get; set;}
    }

}