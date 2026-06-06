using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        
        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids);

        Task<int> CountTotalPagesAsync();

        Task<IEnumerable<T>> GetAllAsync(int? page = 0, params Expression<Func<T, object>>[] includes);

        IQueryable<T> Query(); // Дозволяє сервісу будувати складні запити

        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void Delete(T entity);

        Task SaveChangesAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}