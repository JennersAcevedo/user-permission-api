using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using userPermissionApi.CQRS.Queries;
using userPermissionApi.Data;
using userPermissionApi.Models;
//using userPermissionApi.Domain.Entities;

namespace userPermissionApi.Tests
{
    public class GetAllPermisosQueryHandlerTests
    {

        private readonly N5dbContext _dbContext;

        private readonly GetAllPermisosQueryHandler _handler;

        public GetAllPermisosQueryHandlerTests()
        {
            var options = new DbContextOptionsBuilder<N5dbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new N5dbContext(options);
            _handler = new GetAllPermisosQueryHandler(_dbContext);
        }


        [Fact]
        public async Task Handler_DeberiaRetornarListaDePermisos_CuandoExisten()
        {
            // Arrange
            var permisos = new List<Permiso>
        {
            new Permiso { id = 1, nombreEmpleado = "Juan", apellidoEmpleado = "Gutierrez" ,tipoPermiso = 1 },
            new Permiso { id = 2, nombreEmpleado = "Ana", apellidoEmpleado = "Gutierrez" , tipoPermiso = 1 }
        };

            await _dbContext.Permisos.AddRangeAsync(permisos);
            await _dbContext.SaveChangesAsync();

            var query = new GetAllPermisosQuery();

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Contains(resultado, p => p.nombreEmpleado == "Juan");
            Assert.Contains(resultado, p => p.nombreEmpleado == "Ana");
        }
    }
}
