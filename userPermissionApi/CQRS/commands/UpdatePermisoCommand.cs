using userPermissionApi.Models;
using MediatR;

namespace userPermissionApi.CQRS.commands
{
    public class UpdatePermisoCommand : IRequest<Permiso>
    {
        public int id { get; set; }
        public string nombreEmpleado { get; set; } = string.Empty;

        public string apellidoEmpleado { get; set; } = string.Empty;

        public int tipoPermiso { get; set; }
        
        public DateOnly fechaPermiso { get; set; }
    }
}
