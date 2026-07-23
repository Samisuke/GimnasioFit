using GimnasioFit.Core.Models;
using GimnasioFit.Core.Common;

namespace GimnasioFit.Core.Services
{
    public interface ISocioService
    {
        Task<Resultado<IEnumerable<Socio>>> GetSociosAsync();
        Task<Resultado<Socio>> GetSocioPorIdAsync(int id);
        Task<Resultado<Socio>> PostSocioAsync(
            string nombreSocio,
            string passSocio,
            string emailSocio,
            bool tarifaPremium
        );
        Task<Resultado<Socio>> PatchSocioAsync(
            int idSocio,
            string? nombreSocio,
            string? emailSocio,
            bool? tarifaPremium
        );
    }
}