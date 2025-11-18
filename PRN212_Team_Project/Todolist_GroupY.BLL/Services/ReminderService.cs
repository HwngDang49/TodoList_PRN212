using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todolist_GroupY.DAL.Entities;
using Todolist_GroupY.DAL.Repositories;

namespace Todolist_GroupY.BLL.Services
{
    public class ReminderService
    {
        // Singleton instance
        private static ReminderService? _instance;
        private static readonly object _lock = new object();

        public static ReminderService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ReminderService();
                        }
                    }
                }
                return _instance;
            }
        }

        // Private constructor để ngăn tạo instance từ bên ngoài
        private ReminderService() { }

        private readonly TodoRepo _repo = new();

        //HashSet lưu TodoId đã nhắc rồi để tránh duplicate
        private readonly HashSet<int> _notifiedTodoIds = new();

        public List<Todo> GetTodosNeedingReminder(int userId)
        {
            //1. Lấy tất cả pending reminder từ database
            List<Todo> pendingTodos = _repo.GetPendingReminders(userId);

            //2. Tạo list kết quả
            List<Todo> todosToNotify = new();

            //3. Lọc bỏ những todo đã nhắc rồi
            foreach (Todo todo in pendingTodos)
            {
                // Kiểm tra TodoId có trong HashSet chưa?
                if (!_notifiedTodoIds.Contains(todo.TodoId))
                {
                    // Chưa nhắc → Thêm vào list kết quả
                    todosToNotify.Add(todo);

                    // Đánh dấu đã nhắc
                    _notifiedTodoIds.Add(todo.TodoId);
                }
                // Đã nhắc rồi → Bỏ qua
            }
            return todosToNotify;
        }
        public void ClearNotificationHistory()
        {
            // Xóa toàn bộ lịch sử đã nhắc
            // Gọi khi: logout, restart app
            _notifiedTodoIds.Clear();
        }
        public void RemoveNotifiedTodoId(int todoId)
        {
            // Xóa một TodoId khỏi HashSet
            // Gọi khi user hoàn thành hoặc xóa snoze
            _notifiedTodoIds.Remove(todoId);
        }
    }
}
