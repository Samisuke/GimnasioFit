namespace GimnasioFit.Api.Dtos
{
    public class ClaseCreateDto
    {

        public string Nombre {get; set;} = string.Empty;
        public string Descripcion {get; set;} = string.Empty;
        public int CapacidadMaxima {get; set;}
        public int ProfesorId {get; set;}

    }
}
