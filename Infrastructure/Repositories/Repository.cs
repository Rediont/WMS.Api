using Domain.Interface;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ApplicationDbContext = Infrastructure.DataBase.ApplicationDbContext;

namespace Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context) 
        {
            _context = context; 
        }

        public async Task<IEnumerable<T>> GetAllAsync(int? page = 0, params Expression<Func<T, object>>[] includes)
        {
            const int pageSize = 20;
            int pageIndex = page ?? 0;

            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes) query = query.Include(include);

            return await query
             .Skip(pageIndex * pageSize)
             .Take(pageSize)
             .ToListAsync();
        }

        public async Task<int> CountTotalPagesAsync()
        {
            const int pageSize = 20;
            int totalCount = await _context.Set<T>().CountAsync();
            return (int)Math.Ceiling((double)totalCount / pageSize);
        }

        public IQueryable<T> Query() 
        {
           return _context.Set<T>().AsQueryable(); 
        }

        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes) query = query.Include(include);
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Set<T>()
                    .Where(p => ids.Contains(p.Id))
                    .ToListAsync();
        }

        public async Task AddAsync(T entity) 
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities) 
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)   
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task SaveChangesAsync() 
        {
            await _context.SaveChangesAsync(); 
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }
    }
}
