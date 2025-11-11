using Todolist_GroupY.DAL.Entities;
using Todolist_GroupY.DAL.Repositories;

namespace Todolist_GroupY.BLL.Services
{
    public class TodoService
    {
        private TodoRepo _repo = new();
        public List<Todo> GetTodos()
        {
            return _repo.GetAll();
        }
        public void DeleteTodos(Todo obj)
        {
            _repo.Delete(obj);
        }

        public void UpdateTodos(Todo obj)
        {
            _repo.Update(obj);
        }

        public void CreateTodos(Todo obj)
        {
            _repo.Create(obj);
        }
    }

}
