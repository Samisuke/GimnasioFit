using GimnasioFit.Core.Models;

namespace GimnasioFit.Core.Repositories
{
    public interface ISocioRepository
    {
        Task<IEnumerable<Socio>> ObtenerTodosAsync();
        Task<Socio?> ObtenerSocioPorIdAsync(int id);
        Task AgregarSocioAsync(Socio socio);
        Task<bool> GuardarCambiosAsync();
        Task<Socio?> ObtenerSocioPorEmailAsync(string email);
    }
}