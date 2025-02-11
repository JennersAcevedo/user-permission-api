using Moq;
using userPermissionApi.CQRS.commands;
using userPermissionApi.Repositories;
using userPermissionApi.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using userPermissionApi.Data;

namespace userPermissionApi.Tests
{
    public class CreatePermisoHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRepository<Permiso, int>> _mockPermisoRepository;
        private readonly CreatePermisoHandler _handler;

        public CreatePermisoHandlerTests()
        {
            // Configuración de los mocks
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPermisoRepository = new Mock<IRepository<Permiso, int>>();

            // Asignar el repositorio simulado al UnitOfWork simulado
            _mockUnitOfWork.Setup(uow => uow.PermisoRepository).Returns(_mockPermisoRepository.Object);

            // Crear el handler con la unidad de trabajo simulada
            _handler = new CreatePermisoHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_DeberiaCrearPermiso_CuandoDatosValidos()
        {
            // Crear el comando con los datos de prueba
            var command = new CreatePermisoCommand
            {
                nombreEmpleado = "Juan",
                apellidoEmpleado = "Gutierrez",
                tipoPermisoId = 1,
                fechaPermiso = DateOnly.FromDateTime(DateTime.Now) // Convert DateTime to DateOnly
            };

            // Configurar el repositorio simulado para que la inserción devuelva un Task<int> (por ejemplo, 1)
            _mockPermisoRepository.Setup(repo => repo.InsertAsync(It.IsAny<Permiso>())).Returns(Task.FromResult(1)); // Retorna un int

            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.FromResult(1));
            var resultado = await _handler.Handle(command, CancellationToken.None);

            //Verificar que el permiso fue creado correctamente
            Assert.NotNull(resultado);
            Assert.Equal(command.nombreEmpleado, resultado.nombreEmpleado);
            Assert.Equal(command.apellidoEmpleado, resultado.apellidoEmpleado);
            Assert.Equal(command.tipoPermisoId, resultado.tipoPermiso);
            Assert.Equal(command.fechaPermiso, resultado.fechaPermiso);

            // Verificar que se llamó al método InsertAsync del repositorio
            _mockPermisoRepository.Verify(repo => repo.InsertAsync(It.IsAny<Permiso>()), Times.Once);

            // Verificar que se llamó al método SaveChangesAsync del UnitOfWork
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }
    }
}
