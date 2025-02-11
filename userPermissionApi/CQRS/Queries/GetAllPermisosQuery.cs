using MediatR;
using userPermissionApi.Models;

namespace userPermissionApi.CQRS.Queries
{
    public class GetAllPermisosQuery : IRequest<List<Permiso>>
    {
    }
}
