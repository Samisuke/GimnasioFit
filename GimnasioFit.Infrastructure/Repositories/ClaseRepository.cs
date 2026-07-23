using System.Collections;
using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GimnasioFit.Infrastructure.Repositories
{
    public class ClaseRepository : IClaseRepository
    {
        private readonly GimnasioFitDbContext _context;
        public ClaseRepository(GimnasioFitDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Clase>> ObtenerTodasLasClasesAsync()
        {
            return await _context.ListaClases
                .AsNoTracking()
                .Include(c=> c.Profesor)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Clase>> ObtenerTodasLasClasesDeUnNombreAsync(string nombreClase)
        {
            return await _context.ListaClases
                .AsNoTracking()
                .Include(c => c.Profesor)
                .Where(c => c.Nombre == nombreClase)
                .ToListAsync();
        }

        public async Task CrearUnaClaseNuevaAsync(Clase clase)
        {
            await _context.ListaClases.AddAsync(clase);
        }

        public async Task<bool> GuardarCambiosAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Clase?> ObtenerUnaClasePorIdAsync(int id)
        {
            return await _context.ListaClases
                .Include(c => c.Profesor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}