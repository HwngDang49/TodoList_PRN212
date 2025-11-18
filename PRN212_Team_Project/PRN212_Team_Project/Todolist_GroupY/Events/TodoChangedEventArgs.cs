using System;

namespace Todolist_GroupY.Events
{
    /// <summary>
    /// Event arguments cho TodoChanged event
    /// Chứa thông tin về loại thay đổi và UserId liên quan
    /// </summary>
    public class TodoChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Loại thay đổi: Create, Update, Delete
        /// </summary>
        public TodoChangeType ChangeType { get; set; }

        /// <summary>
        /// UserId của user có todo bị thay đổi
        /// Dùng để filter refresh chỉ cho user liên quan
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// TodoId bị thay đổi (optional - để tracking)
        /// </summary>
        public int? TodoId { get; set; }

        public TodoChangedEventArgs(TodoChangeType changeType, int userId, int? todoId = null)
        {
            ChangeType = changeType;
            UserId = userId;
            TodoId = todoId;
        }
    }

    /// <summary>
    /// Enum định nghĩa các loại thay đổi
    /// </summary>
    public enum TodoChangeType
    {
        Created,    // Todo mới được tạo
        Updated,    // Todo được cập nhật (title, due date, completed, etc.)
        Deleted,    // Todo bị xóa
        Completed,  // Todo được đánh dấu hoàn thành (từ NotificationWindow)
        Snoozed     // Todo được hoãn reminder (từ NotificationWindow)
    }
}

