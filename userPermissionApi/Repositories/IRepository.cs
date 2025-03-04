﻿namespace userPermissionApi.Repositories
{
    public interface IRepository<T, TKey> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(TKey id); 
        Task InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
    }
}
