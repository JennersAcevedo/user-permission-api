using MediatR;
using userPermissionApi.Models;
using userPermissionApi.Repositories;

namespace userPermissionApi.CQRS.commands
{
    public class CreatePermisoHandler : IRequestHandler<CreatePermisoCommand, Permiso>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePermisoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Permiso> Handle(CreatePermisoCommand request, CancellationToken cancellationToken)
        {
            var permiso = new Permiso
            {
                nombreEmpleado = request.nombreEmpleado,
                apellidoEmpleado = request.apellidoEmpleado,
                tipoPermiso = request.tipoPermisoId,
                fechaPermiso = request.fechaPermiso
            };

            await _unitOfWork.PermisoRepository.InsertAsync(permiso);
            await _unitOfWork.SaveChangesAsync();

            return permiso;
        }
    }
}
