using GimnasioFit.Core.Common;
using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Core.Services;

namespace GimnasioFit.Infrastructure.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repoReserva;
        private readonly IClaseRepository _repoClase;
        private readonly ISocioRepository _repoSocio;

        public ReservaService(IReservaRepository repoReserva,
            IClaseRepository repoClase,
            ISocioRepository repoSocio)
        {
            _repoReserva = repoReserva;
            _repoClase = repoClase;
            _repoSocio = repoSocio;
        }

        public async Task<Resultado<IEnumerable<Reserva>>> GetReservasDeUnaClaseAsync(int idClase)
        {
            var clasesDb = await _repoClase.ObtenerUnaClasePorIdAsync(idClase);
            if (clasesDb is null) return Resultado<IEnumerable<Reserva>>.Mal("ERROR. No se encuentra la clase.");

            var reservasDb = await _repoReserva.ObtenerReservasDeUnaClaseAsync(idClase);
            return Resultado<IEnumerable<Reserva>>.Bien(reservasDb);
        }

        public async Task<Resultado<IEnumerable<Reserva>>> GetReservasDeUnSocioAsync(int idSocio)
        {
            var socioDb = await _repoSocio.ObtenerSocioPorIdAsync(idSocio);
            if (socioDb is null) return Resultado<IEnumerable<Reserva>>.Mal("ERROR. No se encuentra el socio.");

            var reservasDb = await _repoReserva.ObtenerReservasDeUnSocioAsync(idSocio);
            return Resultado<IEnumerable<Reserva>>.Bien(reservasDb);
        }

        public async Task<Resultado<Reserva>> PostReservaAsync(string email, int claseId)
        {
            var socio = await _repoSocio.ObtenerSocioPorEmailAsync(email);
            if (socio is null) return Resultado<Reserva>.Mal("ERROR. El usuario no esta registrado.");

            var clase = await _repoClase.ObtenerUnaClasePorIdAsync(claseId);
            if (clase is null) return Resultado<Reserva>.Mal("ERROR. La clase no existe.");

            var reservasTotalesClase = await _repoReserva.ObtenerNumeroReservasDeUnaClaseAsync(claseId);
            if (reservasTotalesClase >= clase.CapacidadMaxima) return Resultado<Reserva>.Mal("ERROR. La clase esta completa.");

            bool esRepetido = await _repoReserva.ObtenerSiSocioEsRepetidoAsync(socio.Id, clase.Id);
            if (esRepetido) return Resultado<Reserva>.Mal("ERROR. Ya estas apuntado en esta clase.");

            var reservaFinal = new Reserva
            {
                FechaReserva = DateTime.UtcNow,
                Socio = socio,
                Clase = clase
            };

            await _repoReserva.CrearReservaAsync(reservaFinal);
            var guardadoExitoso = await _repoReserva.GuardarCambiosAsync();
            if (!guardadoExitoso) return Resultado<Reserva>.Mal("ERROR. No se ha podido guardar la reserva.");

            return Resultado<Reserva>.Bien(reservaFinal);
        }   
    }
}