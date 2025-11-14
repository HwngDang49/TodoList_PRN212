using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Todolist_GroupY.DAL.Entities;

namespace Todolist_GroupY
{
    public class NotificationManager
    {
        // Singleton pattern
        private static NotificationManager _instance;

        // List lưu các notification đang hiển thị
        private List<NotificationWindow> _activeNotifications = new();

        // Khoảng cách giữa các notification (px)
        private const int NOTIFICATION_SPACING = 10;

        // Singleton Instance
        public static NotificationManager Instance
        {
            get
            {
                // Lazy initialization
                if (_instance == null)
                    _instance = new NotificationManager();
                return _instance;
            }
        }
        public void ShowNotification(Todo todo)
        {
            // Đảm bảo chạy trên UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                // [1] Tạo notification window mới
                NotificationWindow notification = new NotificationWindow(todo);

                // [2] Đăng ký event khi notification đóng
                notification.Closed += (s, e) =>
                {
                    // Xóa khỏi list
                    _activeNotifications.Remove(notification);

                    // Tính toán lại vị trí các notification còn lại
                    RepositionNotifications();
                };

                // [3] Thêm vào list active TRƯỚC KHI reposition
                _activeNotifications.Add(notification);

                // [4] Tính toán lại vị trí tất cả notifications (bao gồm cái mới)
                RepositionNotifications();

                // [5] Hiển thị window
                notification.Show();
            });
        }

        public void RepositionNotifications()
        {
            // Lấy kích thước màn hình làm việc
            double screenHeight = SystemParameters.WorkArea.Height;
            double screenWidth = SystemParameters.WorkArea.Width;

            // Bắt đầu từ đáy màn hình
            double currentBottom = screenHeight - NOTIFICATION_SPACING;

            // Xếp chồng từ dưới lên (Reverse để notification mới nhất ở dưới)
            foreach (var notification in _activeNotifications.AsEnumerable().Reverse())
            {
                // Đặt vị trí X (góc phải)
                notification.Left = screenWidth - notification.Width - NOTIFICATION_SPACING;

                // Đặt vị trí Y (xếp chồng từ dưới lên)
                notification.Top = currentBottom - notification.Height;

                // Di chuyển lên trên cho notification tiếp theo
                currentBottom -= (notification.Height + NOTIFICATION_SPACING);
            }
        }
    }
}
