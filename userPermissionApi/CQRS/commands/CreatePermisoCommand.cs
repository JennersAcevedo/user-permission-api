
using MediatR;
using userPermissionApi.Models;

namespace userPermissionApi.CQRS.commands
{
    public class CreatePermisoCommand : IRequest<Permiso>
    {
        public string nombreEmpleado { get; set; } 
        public string apellidoEmpleado { get; set; }
        public int tipoPermisoId { get; set; }
        public DateOnly fechaPermiso { get; set; }
    }
}
