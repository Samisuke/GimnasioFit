using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Core.Services;
using GimnasioFit.Core.Common;

namespace GimnasioFit.Infrastructure.Services
{
    public class SocioService : ISocioService
    {
        private readonly ISocioRepository _repoSocio;

        public SocioService(ISocioRepository repoSocio)
        {
            _repoSocio = repoSocio;
        }

        public async Task<Resultado<IEnumerable<Socio>>> GetSociosAsync()
        {
            var sociosDb = await _repoSocio.ObtenerTodosAsync();
            if (!sociosDb.Any()) return Resultado<IEnumerable<Socio>>.Mal("ERROR. El socio no existe.");

            return Resultado<IEnumerable<Socio>>.Bien(sociosDb);
        }

        public async Task<Resultado<Socio>> GetSocioPorIdAsync(int id)
        {
            var socioDb = await _repoSocio.ObtenerSocioPorIdAsync(id);
            if (socioDb is null) return Resultado<Socio>.Mal("ERROR. No se encuentra el usuario.");

            return Resultado<Socio>.Bien(socioDb);
        }

        public async Task<Resultado<Socio>> PostSocioAsync(
            string nombreSocio,
            string passSocio,
            string emailSocio,
            bool tarifaPremium
        )
        {
            var emailRepetido = await _repoSocio.ObtenerSocioPorEmailAsync(emailSocio);
            if (emailRepetido is not null && emailRepetido.Email == emailSocio) return Resultado<Socio>.Mal("ERROR. Este email ya esta registrado.");


            var socioNuevo = new Socio
            {
                Nombre = nombreSocio,
                Pass = BCrypt.Net.BCrypt.HashPassword(passSocio),
                Email = emailSocio,
                FechaAlta = DateTime.UtcNow,
                TarifaPremium = tarifaPremium,
            };

            await _repoSocio.AgregarSocioAsync(socioNuevo);
            var guardadoExitoso = await _repoSocio.GuardarCambiosAsync();
            if (!guardadoExitoso) return Resultado<Socio>.Mal("ERROR. El socio no se ha podido guardar.");

            return Resultado<Socio>.Bien(socioNuevo);
        }
        public async Task<Resultado<Socio>> PatchSocioAsync(
            int idSocio,
            string? nombreSocio,
            string? emailSocio,
            bool? tarifaPremium
        )
        {
            int numeroCambios = 0;

            var socioDb = await _repoSocio.ObtenerSocioPorIdAsync(idSocio);
            if (socioDb is null) return Resultado<Socio>.Mal("ERROR. No se encuentra el socio   .");

            if (nombreSocio is not null)
            {
                socioDb.Nombre = nombreSocio;
                numeroCambios += 1;
            }

            if (emailSocio is not null)
            {
                var emailRepetido = await _repoSocio.ObtenerSocioPorEmailAsync(emailSocio);
                if (emailRepetido is not null && emailRepetido.Email == emailSocio) return Resultado<Socio>.Mal("ERROR. Este email ya esta registrado.");

                socioDb.Email = emailSocio;
                numeroCambios += 1;
            }

            if (tarifaPremium.HasValue)
            {
                socioDb.TarifaPremium = tarifaPremium.Value;
                numeroCambios += 1;
            }

            if (numeroCambios == 0) return Resultado<Socio>.Mal("ERROR. No se han encontrado cambios que efectuar.");

            var guardadoExitoso = await _repoSocio.GuardarCambiosAsync();
            if (!guardadoExitoso) return Resultado<Socio>.Mal("ERROR. No se han podido guardar los cambios.");

            return Resultado<Socio>.Bien(socioDb);

        }
    }
}