using NSubstitute;
using FluentAssertions;
using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Infrastructure.Services;

namespace GimnasioFit.Tests.Services
{
    public class ClaseServiceTests
    {
        private readonly IClaseRepository _repoClaseMock;
        private readonly ClaseService _service;

        public ClaseServiceTests()
        {
            _repoClaseMock = Substitute.For<IClaseRepository>();
            _service = new ClaseService(_repoClaseMock);
        }

        [Fact]
        public async Task GetTodasLasClasesAsync_CuandoHayClases_DebeRetornarLista()
        {
            var listaClases = new List<Clase>
            {
                new Clase { Id = 1, Nombre = "Spinning", CapacidadMaxima = 15 },
                new Clase { Id = 2, Nombre = "Yoga", CapacidadMaxima = 10 }
            };
            _repoClaseMock.ObtenerTodasLasClasesAsync().Returns(listaClases);

            var resultado = await _service.GetTodasLasClasesAsync();

            resultado.EsCorrecto.Should().BeTrue();
            resultado.Valor.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetTodasLasClasesAsync_CuandoListaEstaVacia_DebeRetornarError()
        {
            _repoClaseMock.ObtenerTodasLasClasesAsync().Returns(new List<Clase>());

            var resultado = await _service.GetTodasLasClasesAsync();

            resultado.EsCorrecto.Should().BeFalse();
            resultado.MensajeError.Should().Contain("No hay clases");
        }

        [Fact]
        public async Task PostUnaClaseAsync_CuandoGuardadoEsCorrecto_DebeRetornarClaseCreada()
        {
            _repoClaseMock.GuardarCambiosAsync().Returns(true);

            var resultado = await _service.PostUnaClaseAsync("Pilates", "Clase suave", 12, 1);

            resultado.EsCorrecto.Should().BeTrue();
            resultado.Valor!.Nombre.Should().Be("Pilates");
            await _repoClaseMock.Received(1).CrearUnaClaseNuevaAsync(Arg.Any<Clase>());
        }
    }
}