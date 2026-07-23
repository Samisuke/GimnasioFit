namespace GimnasioFit.Api.Dtos
{
    public class SocioPatchDto
    {
        public string? Nombre {get; set;} = string.Empty;
        public string? Email {get; set;} = string.Empty;
        public bool? TarifaPremium {get; set;}
    }
}