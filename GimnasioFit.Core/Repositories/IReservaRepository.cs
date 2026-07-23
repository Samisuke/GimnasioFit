using GimnasioFit.Core.Models;

namespace GimnasioFit.Core.Repositories
{
    public interface IReservaRepository
    {
        Task<Reserva?> ObtenerReservaPorIdAsync(int id);
        Task<IEnumerable<Reserva>> ObtenerReservasDeUnSocioAsync(int id);
        Task<IEnumerable<Reserva>> ObtenerReservasDeUnaClaseAsync(int idClase);
        Task<IEnumerable<Reserva>> ObtenerReservasDeUnProfesorAsync(string nombreProfesor);
        Task CrearReservaAsync(Reserva reserva);
        Task<bool> GuardarCambiosAsync();


        Task<int> ObtenerNumeroReservasDeUnaClaseAsync(int idClase);
        Task<bool> ObtenerSiSocioEsRepetidoAsync(int idSocio, int idClase);
    }
}