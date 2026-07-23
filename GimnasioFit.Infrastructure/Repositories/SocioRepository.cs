using System.Drawing;
using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GimnasioFit.Infrastructure.Repositories
{
    public class SocioRepository : ISocioRepository
    {
        private readonly GimnasioFitDbContext _context;
        public SocioRepository(GimnasioFitDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Socio>> ObtenerTodosAsync()
        {
            return await _context.ListaSocios
            .AsNoTracking()
            .Include(s => s.ReservasActivas)
            .ToListAsync();
        }

        public async Task<Socio?> ObtenerSocioPorIdAsync(int id)
        {
            return await _context.ListaSocios
            .Include(s => s.ReservasActivas)
            .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AgregarSocioAsync(Socio socio)
        {
            await _context.ListaSocios.AddAsync(socio);
        }

        public async Task<bool> GuardarCambiosAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Socio?> ObtenerSocioPorEmailAsync(string email)
        {
            return await _context.ListaSocios
            .FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}