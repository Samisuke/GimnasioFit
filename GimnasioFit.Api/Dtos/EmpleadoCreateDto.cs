namespace GimnasioFit.Api.Dtos
{
    public class EmpleadoCreateDto
    {
        public string Nombre {get; set;} = string.Empty;
        public string Pass {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public string Puesto {get; set;} = string.Empty;
        public int NivelAcceso {get; set;}
    }
}