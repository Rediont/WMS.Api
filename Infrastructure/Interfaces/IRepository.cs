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

        Task<IEnumerable<T>> GetAllAsync();

        IQueryable<T> Query(); // Дозволяє сервісу будувати складні запити

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task SaveChangesAsync();
    }
}