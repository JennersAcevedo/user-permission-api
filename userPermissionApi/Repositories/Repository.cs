using Microsoft.EntityFrameworkCore;
using userPermissionApi.Repositories;
using userPermissionApi.Models;
using userPermissionApi.Data;

namespace userPermissionApi.Repositories
{
    public class Repository<T, TKey> : IRepository<T, TKey> where T : class
    {

        private readonly N5dbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(N5dbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(TKey id) => await _dbSet.FindAsync(id);

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
