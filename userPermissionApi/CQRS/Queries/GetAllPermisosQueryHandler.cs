using userPermissionApi.Models;
using userPermissionApi.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace userPermissionApi.CQRS.Queries
{
    public class GetAllPermisosQueryHandler : IRequestHandler<GetAllPermisosQuery, List<Permiso>>
    {

        private readonly N5dbContext _context;

        public GetAllPermisosQueryHandler(N5dbContext context)
        {
            _context = context;
        }

        public async Task<List<Permiso>> Handle(GetAllPermisosQuery request, CancellationToken cancellationToken)
        {
            return await _context.Permisos.ToListAsync(cancellationToken);
        }
    }
}
