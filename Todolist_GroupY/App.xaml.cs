using Hardcodet.Wpf.TaskbarNotification;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Todolist_GroupY.BLL.Services;
using Todolist_GroupY.DAL.Entities;

namespace Todolist_GroupY
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {   // khai bao bien notifyIcon de hien thi tray icon
        private TaskbarIcon? notifyIcon;

        // Field lưu timer để check reminder định kỳ
        private DispatcherTimer? reminderTimer;

        // Field lưu service xử lý logic reminder
        private ReminderService? reminderService;

        // Static property lưu userId của user đang login
        // Static để có thể truy cập từ bất kỳ đâu: App.CurrentUserId
        public static int? CurrentUserId { get; set; }
        protected override void OnStartup(StartupEventArgs e) // method chay khi ung dung duoc khoi dong
        {   //goi code cua class cha truoc (bat buoc)
            base.OnStartup(e);

            // Khởi tạo ReminderService
            reminderService = ReminderService.Instance;

            // Tạo DispatcherTimer - timer chạy trên UI thread
            reminderTimer = new DispatcherTimer();

            // Đặt interval 30 giây - mỗi 30s sẽ check 1 lần
            reminderTimer.Interval = TimeSpan.FromSeconds(30);

            // Đăng ký event handler cho sự kiện Tick
            reminderTimer.Tick += ReminderTimer_Tick;

            // Bắt đầu timer
            reminderTimer.Start();

            // khoi tao tray icon
            notifyIcon = (TaskbarIcon?)FindResource("NotifyIcon");// Tìm resource có key = "NotifyIcon" trong App.xaml
        }

        // Event handler cho sự kiện Tick
        private void ReminderTimer_Tick(object? sender, EventArgs e)
        {
            // Guard clause: Nếu chưa login hoặc service null → return
            if (CurrentUserId == null || reminderService == null)
                return;

            // Lấy danh sách todos cần nhắc nhở
            List<Todo> todosToNotify = reminderService
                .GetTodosNeedingReminder(CurrentUserId.Value);

            // Hiển thị notification cho từng todo
            foreach (Todo todo in todosToNotify)
            {
                NotificationManager.Instance.ShowNotification(todo);
            }
        }
        // CHỈ chạy khi thực sự thoát app (qua menu Exit)
        protected override void OnExit(ExitEventArgs e)
        {
            // Dọn dẹp khi thoát app
            reminderTimer?.Stop();  // Dừng timer
            reminderService?.ClearNotificationHistory();  // Xóa lịch sử
            notifyIcon?.Dispose(); // Xóa icon khỏi tray
            base.OnExit(e);
        }

        //method chay khi double click vao tray icon
        private void NotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ShowAppropriateWindow();
        }

        // Menu: Mở ứng dụng
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            ShowAppropriateWindow();
        }

        // Helper method: Hiển thị window phù hợp dựa trên trạng thái login
        private void ShowAppropriateWindow()
        {
            // Kiểm tra user đã login chưa
            if (CurrentUserId.HasValue)
            {
                // ĐÃ LOGIN → Hiện MainWindow
                ShowOrCreateMainWindow();
            }
            else
            {
                // CHƯA LOGIN → Hiện LoginWindow
                ShowOrCreateLoginWindow();
            }
        }

        // Helper method: Hiển thị hoặc tạo mới MainWindow
        private void ShowOrCreateMainWindow()
        {
            MainWindow? mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (mainWindow != null)
            {
                // Nếu đã có MainWindow → Hiện lên
                if (mainWindow.WindowState == WindowState.Minimized)
                {
                    mainWindow.WindowState = WindowState.Normal;
                }
                mainWindow.Show();
                mainWindow.Activate();
            }
            else
            {
                // Nếu chưa có → Tạo mới MainWindow
                mainWindow = new MainWindow(CurrentUserId!.Value);
                mainWindow.Show();
                mainWindow.Activate();
            }
        }

        // Helper method: Hiển thị hoặc tạo mới LoginWindow
        private void ShowOrCreateLoginWindow()
        {
            LoginWindow? loginWindow = Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault();

            if (loginWindow != null)
            {
                // Nếu đã có LoginWindow → Hiện lên
                if (loginWindow.WindowState == WindowState.Minimized)
                {
                    loginWindow.WindowState = WindowState.Normal;
                }
                loginWindow.Show();
                loginWindow.Activate();
            }
            else
            {
                // Nếu chưa có → Tạo mới LoginWindow
                loginWindow = new LoginWindow();
                loginWindow.Show();
                loginWindow.Activate();
            }
        }

        // Menu: Thoát ứng dụng (CHỈ cách này mới thực sự tắt)
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            // Thoát THẬT SỰ → OnExit() sẽ dispose icon
            Shutdown();
        }

      
     

    }

}
