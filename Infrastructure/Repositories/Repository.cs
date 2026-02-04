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

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
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
    }
}
