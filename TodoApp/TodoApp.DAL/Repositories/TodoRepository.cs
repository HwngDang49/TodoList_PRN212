using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories
{
    public class TodoRepository : IRepository<Todo>
    {

        /*
        *  Create 
        */
        public void Create(Todo entity)
        {
            throw new NotImplementedException();
        }

        /*
        *  Delete 
        */
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        /*
        *  Delete All 
        */
        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        /*
        *  Get All 
        */
        public List<Todo> GetAll()
        {
            throw new NotImplementedException();
        }

        /*
        *  Get By Id 
        */
        public Todo GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        /*
        *  Update 
        */
        public void Update(Todo entity)
        {
            throw new NotImplementedException();
        }
    }
}
