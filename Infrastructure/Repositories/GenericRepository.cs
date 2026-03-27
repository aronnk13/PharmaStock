using System;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces;
using PharmaStock.Models;
namespace PharmaStock.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        PharmaStockContext _context; //represents the whole db.
        DbSet<T> _dbset; //represents a table, whichever we're passing as T.
        public GenericRepository(PharmaStockContext context)
        {
            this._context = context;
            this._dbset = _context.Set<T>();
        }
 
        public async System.Threading.Tasks.Task AddAsync(T obj)
        {
            await _dbset.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var entity = await _dbset.FindAsync(id);
            if(entity != null)
            {
                _dbset.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }
 

        public async Task<T> GetByIdAsync(int id)
        {
            T entity = await _dbset.FindAsync(id);
            if(entity != null)
            {
                return entity;
            }
            return null;
        }

        public void Update(T obj)
        {
            _dbset.Update(obj);
            _context.SaveChanges();
        }
    }
}