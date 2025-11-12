using Hardcodet.Wpf.TaskbarNotification;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Linq;

namespace Todolist_GroupY
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {   // khai bao bien notifyIcon de hien thi tray icon
        private TaskbarIcon? notifyIcon;
        protected override void OnStartup(StartupEventArgs e) // method chay khi ung dung duoc khoi dong
        {   //goi code cua class cha truoc (bat buoc)
            base.OnStartup(e);
            // khoi tao tray icon
            notifyIcon = (TaskbarIcon?)FindResource("NotifyIcon");// Tìm resource có key = "NotifyIcon" trong App.xaml
        }
        // CHỈ chạy khi thực sự thoát app (qua menu Exit)
        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon?.Dispose(); // Xóa icon khỏi tray
            base.OnExit(e);
        }

        //method chat khi double click vao tray icon
        private void NotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            // Tìm xem LoginWindow đã mở chưa
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

        // Menu: Mở ứng dụng
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            //Tìm xem LoginWindow đã mở chưa
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
