using NSubstitute;
using FluentAssertions;
using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Infrastructure.Services;

namespace GimnasioFit.Tests.Services
{
    public class SocioServiceTests
    {
        private readonly ISocioRepository _repoSocioMock;
        private readonly SocioService _socioService;

        public SocioServiceTests()
        {
            _repoSocioMock = Substitute.For<ISocioRepository>();
            _socioService = new SocioService(_repoSocioMock);
        }

        [Fact]
        public async Task GetSocioPorIdAsync_CuandoSocioExiste_DebeRetornarResultadoExitosoConElSocio()
        {
            int socioId = 1;
            var socioSimulado = new Socio
            {
                Id = socioId,
                Nombre = "Carlos Pérez",
                Email = "carlos@test.com",
                FechaAlta = DateTime.UtcNow,
                TarifaPremium = true
            };

            _repoSocioMock.ObtenerSocioPorIdAsync(socioId).Returns(socioSimulado);

            var resultado = await _socioService.GetSocioPorIdAsync(socioId);

            resultado.Should().NotBeNull();
            resultado.EsCorrecto.Should().BeTrue();
            resultado.Valor.Should().NotBeNull();
            resultado.Valor!.Id.Should().Be(socioId);
            resultado.Valor.Nombre.Should().Be("Carlos Pérez");

            await _repoSocioMock.Received(1).ObtenerSocioPorIdAsync(socioId);
        }

        [Fact]
        public async Task GetSocioPorIdAsync_CuandoSocioNoExiste_DebeRetornarResultadoFallido()
        {
            int socioId = 99;

            _repoSocioMock.ObtenerSocioPorIdAsync(socioId).Returns((Socio?)null);

            var resultado = await _socioService.GetSocioPorIdAsync(socioId);

            resultado.Should().NotBeNull();
            resultado.EsCorrecto.Should().BeFalse();
            resultado.Valor.Should().BeNull();
            resultado.MensajeError.Should().Contain("encuentra");
        }
    }
}