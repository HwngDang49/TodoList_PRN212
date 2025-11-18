using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Todolist_GroupY
{
    /// <summary>
    /// Interaction logic for MessageBoxWindow.xaml
    /// </summary>
    public partial class MessageBoxWindow : Window
    {
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.Cancel;

        // Constructor private để chỉ dùng qua hàm Show() tĩnh
        private MessageBoxWindow(string message, string title, MessageBoxButton buttons)
        {
            InitializeComponent();
            this.Title = title;
            MessageText.Text = message;

            // Ẩn hai nút trước
            PrimaryButton.Visibility = Visibility.Collapsed;
            SecondaryButton.Visibility = Visibility.Collapsed;

            // Thiết lập nút dựa trên kiểu MessageBoxButton truyền vào
            if (buttons == MessageBoxButton.OK)
            {
                PrimaryButton.Content = "OK";
                PrimaryButton.Visibility = Visibility.Visible;
                // Nếu chỉ có nút OK, căn nút ra giữa
                ((StackPanel)PrimaryButton.Parent).HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (buttons == MessageBoxButton.OKCancel)
            {
                PrimaryButton.Content = "OK";
                SecondaryButton.Content = "Hủy";
                PrimaryButton.Visibility = Visibility.Visible;
                SecondaryButton.Visibility = Visibility.Visible;
            }
            else if (buttons == MessageBoxButton.YesNo)
            {
                PrimaryButton.Content = "Có"; // Nút chính cho Yes
                SecondaryButton.Content = "Không"; // Nút phụ cho No
                PrimaryButton.Visibility = Visibility.Visible;
                SecondaryButton.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Xử lý kết quả dựa trên nội dung nút
            if (clickedButton.Content.ToString() == "OK")
            {
                Result = MessageBoxResult.OK;
            }
            else if (clickedButton.Content.ToString() == "Có")
            {
                Result = MessageBoxResult.Yes;
            }
            else if (clickedButton.Content.ToString() == "Hủy")
            {
                Result = MessageBoxResult.Cancel;
            }
            else if (clickedButton.Content.ToString() == "Không")
            {
                Result = MessageBoxResult.No;
            }

            this.Close();
        }

        /// <summary>
        /// Phương thức tĩnh để gọi Custom Message Box thay thế cho MessageBox.Show()
        /// </summary>
        public static MessageBoxResult Show(string message, string title, MessageBoxButton buttons = MessageBoxButton.OK)
        {
            MessageBoxWindow customBox = new MessageBoxWindow(message, title, buttons);
            customBox.ShowDialog();
            return customBox.Result;
        }
    }
}
