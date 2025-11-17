using System;

namespace Todolist_GroupY.Events
{
    /// <summary>
    /// Centralized Event Bus cho Todo changes
    /// Singleton pattern - chỉ có 1 instance duy nhất trong app
    /// </summary>
    public class TodoEventBus
    {
        // Singleton instance
        private static TodoEventBus? _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Event được raise khi có bất kỳ thay đổi nào về Todo
        /// Subscribers (MainWindow) sẽ nhận được thông báo
        /// </summary>
        public event EventHandler<TodoChangedEventArgs>? TodoChanged;

        /// <summary>
        /// Singleton Instance - Thread-safe lazy initialization
        /// </summary>
        public static TodoEventBus Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TodoEventBus();
                        }
                    }
                }
                return _instance;
            }
        }

        // Private constructor để ngăn tạo instance từ bên ngoài
        private TodoEventBus() { }

        /// <summary>
        /// Publish TodoChanged event
        /// Gọi từ bất kỳ component nào khi có thay đổi Todo
        /// </summary>
        /// <param name="changeType">Loại thay đổi (Created/Updated/Deleted/etc.)</param>
        /// <param name="userId">UserId của user có todo bị thay đổi</param>
        /// <param name="todoId">TodoId bị thay đổi (optional)</param>
        public void PublishTodoChanged(TodoChangeType changeType, int userId, int? todoId = null)
        {
            // Tạo event args
            var args = new TodoChangedEventArgs(changeType, userId, todoId);

            // Raise event - tất cả subscribers sẽ nhận được
            TodoChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Subscribe to TodoChanged event
        /// </summary>
        /// <param name="handler">Event handler function</param>
        public void Subscribe(EventHandler<TodoChangedEventArgs> handler)
        {
            TodoChanged += handler;
        }

        /// <summary>
        /// Unsubscribe from TodoChanged event
        /// QUAN TRỌNG: Phải gọi khi window close để tránh memory leak
        /// </summary>
        /// <param name="handler">Event handler function</param>
        public void Unsubscribe(EventHandler<TodoChangedEventArgs> handler)
        {
            TodoChanged -= handler;
        }
    }
}

