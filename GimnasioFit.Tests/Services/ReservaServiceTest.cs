using NSubstitute;
using FluentAssertions;
using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Infrastructure.Services;
using GimnasioFit.Core.Services;

namespace GimnasioFit.Tests.Services
{
public class ReservaServiceTests
    {
        private readonly IReservaRepository _repoReservaMock;
        private readonly IClaseRepository _repoClaseMock;
        private readonly ISocioRepository _repoSocioMock;
        private readonly ReservaService _service;

        public ReservaServiceTests()
        {
            _repoReservaMock = Substitute.For<IReservaRepository>();
            _repoClaseMock = Substitute.For<IClaseRepository>();
            _repoSocioMock = Substitute.For<ISocioRepository>();
            _service = new ReservaService(_repoReservaMock, _repoClaseMock, _repoSocioMock);
        }

        [Fact]
        public async Task PostReservaAsync_CuandoSocioNoExiste_DebeRetornarError()
        {
            string email = "noexiste@test.com";
            int claseId = 1;
            _repoSocioMock.ObtenerSocioPorEmailAsync(email)
                .Returns((Socio?)null);

            var resultado = await _service.PostReservaAsync(email,claseId);

            resultado.EsCorrecto.Should().BeFalse();
            resultado.MensajeError.Should().Contain("registrado");
        }  

        [Fact]
        public async Task PostReservaAsync_CuandoClaseEstaLlena_DebeRetornarError()
        {
            string email = "clasellena@test.com";
            int claseId = 1;
            var socio = new Socio{Id = 10, Email = email};
            var clase = new Clase{Id = claseId, CapacidadMaxima = 20};

            _repoSocioMock.ObtenerSocioPorEmailAsync(email)
                .Returns(socio);
            _repoClaseMock.ObtenerUnaClasePorIdAsync(claseId)
                .Returns(clase);
            _repoReservaMock.ObtenerNumeroReservasDeUnaClaseAsync(claseId)
                .Returns(20);

            var resultado = await _service.PostReservaAsync(email, claseId);

            resultado.EsCorrecto.Should().BeFalse();
            resultado.MensajeError.Should().Contain("completa");
        }

        [Fact]
        public async Task PostReservaAsync_CuandoSocioYaTieneReserva_DebeRetornarError()
        {
            string email = "socioregistrado@test.com";
            int claseId = 1;
            var socio = new Socio{Id = 2, Email = email};
            var clase = new Clase{Id = claseId, CapacidadMaxima = 20};

            _repoSocioMock.ObtenerSocioPorEmailAsync(email)
                .Returns(socio);
            _repoClaseMock.ObtenerUnaClasePorIdAsync(claseId)
                .Returns(clase);
            _repoReservaMock.ObtenerSiSocioEsRepetidoAsync(socio.Id, clase.Id)
                .Returns(true);

            var resultado = await _service.PostReservaAsync(email, claseId);

            resultado.EsCorrecto.Should().BeFalse();
            resultado.MensajeError.Should().Contain("apuntado");
        }

        [Fact]
        public async Task PostReservaAsync_CuandoTodoEsCorrecto_DebeCrearReservaExitosamente()
        {
            string email = "socioregistrado@test.com";
            int claseId = 1;
            var socio = new Socio{Id = 2, Email = email};
            var clase = new Clase{Id = claseId, CapacidadMaxima = 20};

            _repoSocioMock.ObtenerSocioPorEmailAsync(email)
                .Returns(socio);
            _repoClaseMock.ObtenerUnaClasePorIdAsync(claseId)
                .Returns(clase);
            _repoReservaMock.ObtenerNumeroReservasDeUnaClaseAsync(claseId)
                .Returns(5);
            _repoReservaMock.ObtenerSiSocioEsRepetidoAsync(socio.Id, clase.Id)
                .Returns(false);
            _repoReservaMock.GuardarCambiosAsync()
                .Returns(true);

            var resultado = await _service.PostReservaAsync(email, claseId);

            resultado.EsCorrecto.Should().BeTrue();
            resultado.MensajeError.Should().BeEmpty();

            await _repoReservaMock.Received(1).CrearReservaAsync(Arg.Any<Reserva>());
            await _repoReservaMock.Received(1).GuardarCambiosAsync();
        }

        [Fact]
        public async Task PostReservaAsync_CuandoClaseNoExiste_DebeRetornarError()
        {
            string email = "socio@test.com";
            int claseId = 99;
            var socio = new Socio { Id = 1, Email = email };

            _repoSocioMock.ObtenerSocioPorEmailAsync(email).Returns(socio);
            _repoClaseMock.ObtenerUnaClasePorIdAsync(claseId).Returns((Clase?)null);

            var resultado = await _service.PostReservaAsync(email, claseId);

            resultado.EsCorrecto.Should().BeFalse();
            resultado.MensajeError.Should().Contain("no existe");
        }

        [Fact]
        public async Task PostReservaAsync_CuandoGuardarCambiosFalla_DebeRetornarError()
        {
            string email = "socio@test.com";
            int claseId = 1;
            var socio = new Socio
            {
                Id = 1,
                Email = email
            };
            var clase = new Clase
            {
                Id = claseId,
                CapacidadMaxima = 20
            };

            _repoSocioMock.ObtenerSocioPorEmailAsync(email)
                .Returns(socio);
            _repoClaseMock.ObtenerUnaClasePorIdAsync(claseId)
                .Returns(clase);
            _repoReservaMock.ObtenerNumeroReservasDeUnaClaseAsync(claseId)
                .Returns(5);
            _repoReservaMock.ObtenerSiSocioEsRepetidoAsync(socio.Id, clase.Id)
                .Returns(false);
            _repoReservaMock.GuardarCambiosAsync()
                .Returns(false);

            var resultado = await _service.PostReservaAsync(email, claseId);

            resultado.EsCorrecto.Should().BeFalse();
            resultado.MensajeError.Should().Contain("guardar");
        }
    }
}