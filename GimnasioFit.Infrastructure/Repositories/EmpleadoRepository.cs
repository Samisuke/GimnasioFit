using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GimnasioFit.Infrastructure.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly GimnasioFitDbContext _context;
        public EmpleadoRepository(GimnasioFitDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleado>> ObtenerTodosAsync()
        {
            return await _context.ListaEmpleados
            .AsNoTracking()
            .Include(e => e.ClasesProfesor)
            .ToListAsync();
        }

        public async Task<Empleado?> ObtenerPorIdAsync(int id)
        {
            return await _context.ListaEmpleados
            .Include(e => e.ClasesProfesor)
            .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CrearEmpleadoAsync(Empleado empleado)
        {
            await _context.ListaEmpleados.AddAsync(empleado);   
        }

        public async Task<bool> GuardarCambiosAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Empleado?> ObtenerEmpleadoPorEmailAsync(string email)
        {
            return await _context.ListaEmpleados
            .Include(e => e.ClasesProfesor)
            .FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}