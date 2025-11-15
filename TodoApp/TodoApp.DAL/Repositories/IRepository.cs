using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.DAL.Repositories
{
    public interface IRepository<T>
    {
        T GetById(Guid id);
        List<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        void Delete(Guid id);
        void DeleteAll();
    }
}
