using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Todolist_GroupY.BLL.Services;
using Todolist_GroupY.DAL.Entities;
using Todolist_GroupY.Events;

namespace Todolist_GroupY
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        // Lưu todo object để xử lý Snooze/Complete
        private Todo _todo;

        // Service để update todo
        private TodoService _todoService = new();

        // Event bus để publish TodoChanged event
        private TodoEventBus _eventBus = TodoEventBus.Instance;

        public NotificationWindow(Todo todo)
        {
            InitializeComponent();

            // Lưu todo
            _todo = todo;

            // Hiển thị title lên UI
            TaskTitleText.Text = todo.Title;
        }
        public NotificationWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Event này chạy SAU KHI window đã render xong

            // ❌ KHÔNG GỌI PositionWindow() ở đây
            // Vì NotificationManager đã quản lý positioning rồi
            // Nếu gọi PositionWindow() sẽ ghi đè vị trí → tất cả notifications đều ở cùng 1 chỗ

            // Chỉ phát âm thanh
            SystemSounds.Asterisk.Play();
        }
        private void PositionWindow()
        {
            // Lấy kích thước màn hình làm việc (trừ taskbar)
            double screenWidth = SystemParameters.WorkArea.Width;
            double screenHeight = SystemParameters.WorkArea.Height;

            // Đặt ở góc dưới bên phải, cách lề 10px
            Left = screenWidth - Width - 10;
            Top = screenHeight - Height - 10;
        }
        private void SnoozeButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra có ReminderTime không
            if (_todo.ReminderTime.HasValue)
            {
                // Hoãn 5 phút
                _todo.ReminderTime = DateTime.Now.AddMinutes(5);

                // Update vào database
                _todoService.UpdateTodos(_todo);

                //xóa user khỏi Hashset để có thể nhắc lại
                ReminderService.Instance.RemoveNotifiedTodoId(_todo.TodoId);

                // Publish event để MainWindow auto-refresh
                _eventBus.PublishTodoChanged(TodoChangeType.Snoozed, _todo.UserId, _todo.TodoId);

                // Thông báo cho user
                MessageBox.Show("Đã hoãn reminder 5 phút!", "Snooze",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Đóng window
            Close();
        }
        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Đánh dấu hoàn thành
            _todo.IsCompleted = true;

            // Update vào database
            _todoService.UpdateTodos(_todo);

            // Publish event để MainWindow auto-refresh
            _eventBus.PublishTodoChanged(TodoChangeType.Completed, _todo.UserId, _todo.TodoId);

            // Thông báo cho user
            MessageBox.Show("Task đã hoàn thành!", "Complete",
                MessageBoxButton.OK, MessageBoxImage.Information);

            // Đóng window
            Close();
        }

    }
}
