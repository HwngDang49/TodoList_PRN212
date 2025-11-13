using Todolist_GroupY.DAL.Entities;
using System.Linq;
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
        public List<Todo> GetPendingReminders(int userId)
        {
            //1 Tạo  DbContext mới
            _ctx = new();
            //2 Lấy thời gian hiện tại
            DateTime now = DateTime.Now;

            // [3] Query LINQ to Entities
            return _ctx.Todos
                .Where(x =>
                    x.UserId == userId              // Của user này
                    && x.IsCompleted == false       // Chưa hoàn thành
                    && x.ReminderTime.HasValue      // Có set reminder
                    && x.ReminderTime.Value <= now  // Đã tới giờ
                )
                .ToList();  // Execute query, trả về List<Todo>
        }

    }
}
