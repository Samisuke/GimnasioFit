using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GimnasioFit.Infrastructure.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly GimnasioFitDbContext _context;
        public ReservaRepository(GimnasioFitDbContext context)
        {
            _context = context;
        }
        public async Task<Reserva?> ObtenerReservaPorIdAsync(int id)
        {
            return await _context.ListaReservas.Include(r => r.Socio)
                .Include(r => r.Clase)
                .ThenInclude(p => p.Profesor)
                .FirstOrDefaultAsync(r => r.Id == id);        
        }
        public async Task<IEnumerable<Reserva>> ObtenerReservasDeUnSocioAsync(int id)
        {
            return await _context.ListaReservas.Include(r => r.Socio)
                .Include(r => r.Clase)
                .ThenInclude(p => p.Profesor)
                .Where(r => r.SocioId == id).ToListAsync();     
        }
        
        public async Task<IEnumerable<Reserva>> ObtenerReservasDeUnaClaseAsync(int idClase)
        {
            return await _context.ListaReservas.Include(r => r.Socio)
                .Include(r => r.Clase)
                .ThenInclude(p => p.Profesor)
                .Where(r => r.ClaseId == idClase).ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> ObtenerReservasDeUnProfesorAsync(string nombreProfesor)
        {
            return await _context.ListaReservas.Include(r => r.Socio)
                .Include(r => r.Clase)
                .ThenInclude(p => p.Profesor) 
                .Where(r => r.Clase.Profesor.Nombre == nombreProfesor).ToListAsync();
        }


        public async Task CrearReservaAsync(Reserva reserva)
        {
            await _context.ListaReservas.AddAsync(reserva);
        }

        public async Task<bool> GuardarCambiosAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }


        public async Task<int> ObtenerNumeroReservasDeUnaClaseAsync(int idClase)
        {
            return await _context.ListaReservas.CountAsync(r => r.ClaseId == idClase);
        }

        public async Task<bool> ObtenerSiSocioEsRepetidoAsync(int idSocio, int idClase)
        {
            return await _context.ListaReservas.AnyAsync(r => r.SocioId == idSocio && r.ClaseId == idClase);
        }
    }
}
