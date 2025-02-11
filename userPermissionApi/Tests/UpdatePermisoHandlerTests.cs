using Moq;
using userPermissionApi.Models;
using userPermissionApi.Data;
using userPermissionApi.CQRS.commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace userPermissionApi.Tests
{
    public class UpdatePermisoHandlerTests
    {
        private readonly Mock<N5dbContext> _mockDbContext;
        private readonly Mock<DbSet<Permiso>> _mockPermisoDbSet;
        private readonly UpdatePermisoHandler _handler;

        public UpdatePermisoHandlerTests()
        {
   
            _mockDbContext = new Mock<N5dbContext>();
            _mockPermisoDbSet = new Mock<DbSet<Permiso>>();
            _mockDbContext.Setup(m => m.Permisos).Returns(_mockPermisoDbSet.Object);

            _handler = new UpdatePermisoHandler(_mockDbContext.Object);
        }

        [Fact]
        public async Task Handle_DeberiaActualizarPermiso_CuandoPermisoExiste()
        {
            
            var permisoOriginal = new Permiso
            {
                id = 1,
                nombreEmpleado = "Juan",
                apellidoEmpleado = "Gutierrez",
                tipoPermiso = 1,
                fechaPermiso = DateOnly.FromDateTime(DateTime.Now)
            };

            var command = new UpdatePermisoCommand
            {
                id = 1,
                nombreEmpleado = "Carlos",
                apellidoEmpleado = "Lopez",
                tipoPermiso = 2,
                fechaPermiso = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
            };

            
            _mockPermisoDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(permisoOriginal);

            var resultado = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(command.nombreEmpleado, resultado.nombreEmpleado);
            Assert.Equal(command.apellidoEmpleado, resultado.apellidoEmpleado);
            Assert.Equal(command.tipoPermiso, resultado.tipoPermiso);
            Assert.Equal(command.fechaPermiso, resultado.fechaPermiso);

            _mockDbContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarKeyNotFoundException_CuandoPermisoNoExiste()
        {
         
            var command = new UpdatePermisoCommand
            {
                id = 999, 
                nombreEmpleado = "Carlos",
                apellidoEmpleado = "Lopez",
                tipoPermiso = 2,
                fechaPermiso = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
            };

        
            _mockPermisoDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Permiso)null);
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Permiso no encontrado", exception.Message);
        }
    }
}

