using Todolist_GroupY.DAL.Entities;

namespace Todolist_GroupY.DAL.Repositories
{
    public class TodoRepo
    {
        private TodoListNotesDbContext _ctx;
        public List<Todo> GetAll()
        {
            _ctx = new();
            return _ctx.Todos.ToList();

        }
        public List<Todo> GetByUser(int userId)
        {
            _ctx = new();
            return _ctx.Todos.Where(x => x.UserId == userId).ToList();
        }


        public void Delete(Todo obj)
        {
            _ctx = new();
            _ctx.Todos.Remove(obj);
            _ctx.SaveChanges();
        }

        public void Update(Todo obj)
        {
            _ctx = new();
            _ctx.Todos.Update(obj);
            _ctx.SaveChanges();
        }

        public void Create(Todo obj)
        {
            _ctx = new();
            _ctx.Todos.Add(obj);
            _ctx.SaveChanges();
        }

    }
}
