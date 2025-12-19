using Infrastructure.DataBase;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbcontext;

        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }

            _dbcontext.Set<T>().Add(entity);
        }

        public void Remove(Guid id)
        {
            T? entity = _dbcontext.Set<T>().Find(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found");
            }
            _dbcontext.Set<T>().Remove(entity);

        }

        public T GetById(Guid id)
        {
            T? entity = _dbcontext.Set<T>().Find(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found");
            }
            return entity;
        }

        public List<T> GetAll()
        {
            return _dbcontext.Set<T>().ToList();
        }

    }
}
