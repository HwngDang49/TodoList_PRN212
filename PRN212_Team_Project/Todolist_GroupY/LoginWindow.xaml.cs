using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Todolist_GroupY.BLL.Services;
using Todolist_GroupY.DAL.Entities;

namespace Todolist_GroupY
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private UserService _service = new();
        public LoginWindow()
        {
            InitializeComponent();
            //LoadRememberedEmail();
        }

        //private void LoadRememberedEmail()
        //{
        //    // Lấy email đã lưu trong setting
        //    string savedEmail = Properties.Settings.Default.RememberedEmail;
        //    if (!string.IsNullOrWhiteSpace(savedEmail))
        //    {
        //        EmailTextBox.Text = savedEmail;
        //        RememberCheckBox.IsChecked = true;
        //    }
        //}

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string pass = PasswordBox.Password;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Both email and password are required!", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            User? acc = _service.Authenticate(email);
            if (acc == null)
            {
                MessageBox.Show("Email doesn't exist. Sign-up please!", "Wrong credentials", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (acc.Password != pass)
            {
                MessageBox.Show("Invalidated password. Reset it, please!", "Wrong credentials", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //if (RememberCheckBox.IsChecked == true)
            //{
            //    Properties.Settings.Default.RememberedEmail = email;
            //}
            //else
            //{
            //    Properties.Settings.Default.RememberedEmail = string.Empty;
            //}
            App.CurrentUserId = acc.UserId;
            Properties.Settings.Default.Save();
            MainWindow main = new(acc.UserId);
            main.Show();
            this.Close();
        }

        //private void RegisterButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string email = EmailTextBox.Text;
        //    string pass = PasswordBox.Password;

        //    // 1. Validation (Kiểm tra trống)
        //    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
        //    {
        //        MessageBox.Show("Vui lòng nhập email và mật khẩu để đăng ký!", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }

        //    // 2. Kiểm tra email đã tồn tại chưa (dùng lại hàm Authenticate)
        //    User? acc = _service.Authenticate(email);
        //    if (acc != null)
        //    {
        //        MessageBox.Show("Email này đã tồn tại. Vui lòng nhấn 'Login'.", "Lỗi Đăng Ký", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    // 3. Tạo User mới
        //    // Lấy phần trước @ làm UserName mặc định, nếu không có @ thì dùng email làm username
        //    string username = email.Contains("@") ? email.Split('@')[0] : email;

        //    User newUser = new User
        //    {
        //        UserName = username,
        //        Email = email,
        //        Password = pass // LƯU Ý: Bạn đang lưu mật khẩu text
        //    };

        //    // 4. Gọi Service để lưu vào DB
        //    _service.RegisterUser(newUser);

        //    MessageBox.Show("Đăng ký tài khoản thành công! Bây giờ bạn có thể nhấn 'Login' để đăng nhập.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        //}

        //private void RememberCheckBox_Checked(object sender, RoutedEventArgs e)
        //{

        //}

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    e.Cancel = true; // Ngăn không cho cửa sổ đóng
        //    this.Hide(); // Thay vào đó, ẩn cửa sổ
        //}
    }
}
