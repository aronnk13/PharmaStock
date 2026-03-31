using System;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces;
using PharmaStock.Models;
namespace PharmaStock.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
       protected readonly PharmaStockContext _pharmaStockContext; 
       private readonly DbSet<T> _dbset;
        public GenericRepository(PharmaStockContext _pharmaStockContext)
        {
            this._pharmaStockContext = _pharmaStockContext;
            this._dbset = _pharmaStockContext.Set<T>();
        }

        public async System.Threading.Tasks.Task AddAsync(T obj)
        {
            await _dbset.AddAsync(obj);
            await _pharmaStockContext.SaveChangesAsync();
        }
        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var entity = await _dbset.FindAsync(id);
            if (entity != null)
            {
                _dbset.Remove(entity);
                await _pharmaStockContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            T entity = await _dbset.FindAsync(id);
            if (entity != null)
            {
                return entity;
            }
            return null;
        }

        public void Update(T obj)
        {
            _dbset.Update(obj);
            _pharmaStockContext.SaveChanges();
        }
        public async System.Threading.Tasks.Task<bool> UpdateAsync(T obj)
        {
            _pharmaStockContext.Set<T>().Update(obj);
            return await _pharmaStockContext.SaveChangesAsync() > 0;
        }
        public async System.Threading.Tasks.Task Add(T obj)
        {
            await _pharmaStockContext.Set<T>().AddAsync(obj);
            await _pharmaStockContext.SaveChangesAsync();
        }

    }
}