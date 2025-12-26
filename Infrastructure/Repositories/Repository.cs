using Infrastructure.DataBase;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using DbContext = Infrastructure.DataBase.DbContext;

namespace Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        public Repository(DbContext context) 
        {
            _context = context; 
        }

        public IQueryable<T> Query() 
        {
           return _context.Set<T>().AsQueryable(); 
        }

        public async Task<T?> GetByIdAsync(int id) 
        {
            return await _context.Set<T>().FindAsync(id); 
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

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

    }
}
