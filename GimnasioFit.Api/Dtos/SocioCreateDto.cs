namespace GimnasioFit.Api.Dtos
{
    public class SocioCreateDto
    {
        public string Nombre {get; set;} = string.Empty;
        public string Pass {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public bool TarifaPremium {get; set;}
    }
}