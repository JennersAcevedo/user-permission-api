using MediatR;
using userPermissionApi.Models;
using userPermissionApi.Data;   

namespace userPermissionApi.CQRS.commands
{
    public class UpdatePermisoHandler : IRequestHandler<UpdatePermisoCommand, Permiso>
    {
        private readonly N5dbContext _context;

        public UpdatePermisoHandler(N5dbContext context)
        {
            _context = context;
        }

        public async Task<Permiso> Handle(UpdatePermisoCommand request, CancellationToken cancellationToken)
        {
            var permiso = await _context.Permisos.FindAsync(request.id);

            if (permiso == null)
            {
                throw new KeyNotFoundException("Permiso no encontrado");
            }

            // Actualizar los campos
            permiso.nombreEmpleado = request.nombreEmpleado;
            permiso.apellidoEmpleado = request.apellidoEmpleado;
            permiso.tipoPermiso = request.tipoPermiso;
            permiso.fechaPermiso = request.fechaPermiso;

            await _context.SaveChangesAsync(cancellationToken);

            return permiso;
        }
    }
}
