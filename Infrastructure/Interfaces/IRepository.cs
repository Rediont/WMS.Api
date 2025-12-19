using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IRepository<T>
    {
        public void Add(T entity);

        public void Remove(Guid id);
        
        public T GetById(Guid id);
        
        public List<T> GetAll();
    }
}