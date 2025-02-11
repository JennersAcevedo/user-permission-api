using MediatR;
using userPermissionApi.Models;

namespace userPermissionApi.CQRS.Queries
{
    public class GetPermisoByIdQuery: IRequest<Permiso>
    {
        public int Id { get; set; }
    }
}
