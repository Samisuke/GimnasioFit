using NSubstitute;
using FluentAssertions;
using GimnasioFit.Core.Models;
using GimnasioFit.Core.Repositories;
using GimnasioFit.Infrastructure.Services;

namespace GimnasioFit.Tests.Services
{
    public class EmpleadoServiceTests
    {
        private readonly IEmpleadoRepository _repoEmpleadoMock;
        private readonly EmpleadoService _service;

        public EmpleadoServiceTests()
        {
            _repoEmpleadoMock = Substitute.For<IEmpleadoRepository>();
            _service = new EmpleadoService(_repoEmpleadoMock);
        }

        [Fact]
        public async Task PostEmpleado_CuandoEmailYaExiste_DebeRetornarError()
        {
            string emailDuplicado = "admin@gimnasio.com";
            var empleadoExistente = new Empleado { Id = 1, Email = emailDuplicado };

            _repoEmpleadoMock.ObtenerEmpleadoPorEmailAsync(emailDuplicado).Returns(empleadoExistente);

            var resultado = await _service.PostEmpleado("Carlos", "123456", emailDuplicado, "Profesor", 1);

            resultado.EsCorrecto.Should().BeFalse();
            resultado.MensajeError.Should().Contain("El email ya existe");
        }

        [Fact]
        public async Task PatchEmpleado_SinCambios_DebeRetornarError()
        {
            int empleadoId = 1;
            var empleadoDb = new Empleado { Id = empleadoId, Nombre = "Ana" };
            _repoEmpleadoMock.ObtenerPorIdAsync(empleadoId).Returns(empleadoDb);

            var resultado = await _service.PatchEmpleado(empleadoId, null, null, null, null);

            resultado.EsCorrecto.Should().BeFalse();
            resultado.MensajeError.Should().Contain("No se han hecho cambios");
        }
    }
}