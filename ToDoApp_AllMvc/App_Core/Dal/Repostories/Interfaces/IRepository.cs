using App_Core.Models;
using System.Linq.Expressions;

namespace App_Core.Dal.Repostories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    }

}
