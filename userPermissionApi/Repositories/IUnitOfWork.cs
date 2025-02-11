using userPermissionApi.Models;
using userPermissionApi.Repositories;   

namespace userPermissionApi.Repositories
{
    public interface IUnitOfWork :IDisposable
    {
        IRepository<Permiso, int> PermisoRepository { get; }
        IRepository<TipoPermiso, int> TipoPermisoRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
