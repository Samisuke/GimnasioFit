using GimnasioFit.Core.Models;
using GimnasioFit.Core.Common;

namespace GimnasioFit.Core.Services
{
    public interface IReservaService
    {
        Task<Resultado<IEnumerable<Reserva>>> GetReservasDeUnSocioAsync(int idSocio);
        Task<Resultado<IEnumerable<Reserva>>> GetReservasDeUnaClaseAsync(int idClase);
        Task<Resultado<Reserva>>  PostReservaAsync(string email, int claseId);
    }
}