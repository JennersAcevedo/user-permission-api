using userPermissionApi.Models;
using userPermissionApi.Data;   

namespace userPermissionApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly N5dbContext _context;
        public IRepository<Permiso, int> PermisoRepository { get; }
        public IRepository<TipoPermiso, int> TipoPermisoRepository { get; }

        public UnitOfWork(N5dbContext context,
                          IRepository<Permiso, int> permisoRepo,
                          IRepository<TipoPermiso, int> tipoPermisoRepo)
        {
            _context = context;
            PermisoRepository = permisoRepo;
            TipoPermisoRepository = tipoPermisoRepo;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
