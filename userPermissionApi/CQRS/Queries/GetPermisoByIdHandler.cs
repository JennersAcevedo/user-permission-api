using MediatR;
using userPermissionApi.Models;
using userPermissionApi.Repositories;

namespace userPermissionApi.CQRS.Queries
{
    public class GetPermisoByIdHandler : IRequestHandler<GetPermisoByIdQuery, Permiso>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPermisoByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Permiso> Handle(GetPermisoByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.PermisoRepository.GetByIdAsync(request.Id);
        }
    }
}
