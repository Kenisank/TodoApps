using App_Core.Dal.Repostories.Interfaces;
using App_Core.Data;
using App_Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Linq.Expressions;

namespace App_Core.Dal.Repostories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected TodoContext _context { get; set; }
        private readonly DbSet<T> _dbSet;

        public Repository(TodoContext context)
        {
            this._context = context;
            _dbSet = context.Set<T>();
        }



        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this._dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await this._dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            this._dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            this._dbSet.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this._dbSet
                .Where(expression).AsNoTracking();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await this._dbSet.FindAsync(id);
            if (entity != null)
            {
                this._dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
