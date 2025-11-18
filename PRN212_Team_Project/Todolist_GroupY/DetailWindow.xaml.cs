using System.Windows;
using Todolist_GroupY.BLL.Services;
using Todolist_GroupY.DAL.Entities;
using Todolist_GroupY.Events;

namespace Todolist_GroupY
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        private TodoService _todoService = new(); //save
        private TodoEventBus _eventBus = TodoEventBus.Instance;

        public Todo EditedOne { get; set; }
        public Todo IsView { get; set; }

        public int LoggedInUser { get; set; } //lấy user đã đăng nhập

        public DetailWindow()
        {
            InitializeComponent();
        }

        private bool CheckVar()
        {
            // Lấy ngày hiện tại (ngày tạo/ngày lưu)
            DateTime currentDate = DateTime.Now.Date;

            // --- 1. Kiểm tra Tiêu đề (Title) ---
            string title = TitleTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(title) || title.Length < 2 || title.Length > 100)
            {
                MessageBoxWindow.Show("Tiêu đề không được để trống và phải dài từ 2 đến 100 ký tự!", "Lỗi nhập liệu", MessageBoxButton.OK);
                TitleTextBox.Focus();
                return false;
            }

            // --- 2. Kiểm tra Mô tả (Description) ---
            string description = DescTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(description) || description.Length <= 2)
            {
                MessageBoxWindow.Show("Mô tả không được để trống và phải dài hơn 2 ký tự!", "Lỗi nhập liệu", MessageBoxButton.OK);
                DescTextBox.Focus();
                return false;
            }

            // --- 3. Kiểm tra Ngày đáo hạn (DueDate) ---
            if (!DueDatePicker.Value.HasValue)
            {
                MessageBoxWindow.Show("Ngày đáo hạn không được để trống!", "Lỗi nhập liệu", MessageBoxButton.OK);
                DueDatePicker.Focus();
                return false;
            }

            // --- 4. Kiểm tra Thời gian nhắc nhở (ReminderTime) ---
            DateTime? reminderTime = ReminderDatePicker.Value;
            if (!reminderTime.HasValue)
            {
                MessageBoxWindow.Show("Thời gian nhắc nhở không được để trống!", "Lỗi nhập liệu", MessageBoxButton.OK);
                ReminderDatePicker.Focus();
                return false;
            }

            // Bổ sung: Ngày nhắc hẹn không được bé hơn ngày tạo/ngày hiện tại
            // So sánh cả ngày và giờ để chính xác hơn
            if (reminderTime.Value < DateTime.Now)
            {
                MessageBoxWindow.Show("Thời gian nhắc nhở không được bé hơn thời điểm hiện tại!", "Lỗi logic ngày tháng", MessageBoxButton.OK);
                ReminderDatePicker.Focus();
                return false;
            }

            // Nếu tất cả đều hợp lệ
            return true;
        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (EditedOne != null)
            {
                //fill data từ selected vào các ô nhập
                TitleTextBox.Text = EditedOne.Title.ToString();
                DescTextBox.Text = EditedOne.Description;
                DueDatePicker.Value = EditedOne.DueDate;
                ReminderDatePicker.Value = EditedOne.ReminderTime;
                CompletedCheckBox.IsChecked = EditedOne.IsCompleted;
            }
            if (IsView != null)
            {
                Console.WriteLine(IsView.Description);
                //fill data từ selected vào các ô nhập
                TitleTextBox.Text = IsView.Title.ToString();
                DescTextBox.Text = IsView.Description;
                DueDatePicker.Value = IsView.DueDate;
                ReminderDatePicker.Value = IsView.ReminderTime;
                CompletedCheckBox.IsChecked = IsView.IsCompleted;
                //disable tất cả ô nhập
                TitleTextBox.IsEnabled = false;
                DescTextBox.IsEnabled = false;
                DueDatePicker.IsEnabled = false;
                ReminderDatePicker.IsEnabled = false;
                SaveButton.Visibility = Visibility.Hidden;
                CancelButton.Visibility = Visibility.Hidden;
            }

            if (EditedOne == null && IsView == null)
            {
                EditedOne = new Todo();
                EditedOne.UserId = LoggedInUser;
            }
        }

        // Trong DetailWindow.xaml.cs
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckVar())
            {
                return; // Dừng lại nếu dữ liệu không hợp lệ
            }

            try
            {
                
                Todo objToSave;
                TodoChangeType changeType;

                if (EditedOne.TodoId > 0)
                {
                    objToSave = EditedOne;
                    changeType = TodoChangeType.Updated;
                }
                else
                {
                    objToSave = EditedOne;
                    changeType = TodoChangeType.Created;
                }

                objToSave.UserId = LoggedInUser;
                objToSave.Title = TitleTextBox.Text;
                objToSave.Description = DescTextBox.Text;
                objToSave.DueDate = DueDatePicker.Value;
                objToSave.ReminderTime = ReminderDatePicker.Value;
                objToSave.IsCompleted = CompletedCheckBox.IsChecked ?? false;

                if (changeType == TodoChangeType.Updated)
                {
                    _todoService.UpdateTodos(objToSave);
                }
                else
                {
                    _todoService.CreateTodos(objToSave);
                }

                _eventBus.PublishTodoChanged(changeType, LoggedInUser, objToSave.TodoId);

                this.Close();
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi chi tiết hơn
                MessageBoxWindow.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBoxWindow.Show(
        "Mọi thông tin thay đổi sẽ không được Lưu \n",
        "Cảnh báo",
        MessageBoxButton.OKCancel);

            if (answer == MessageBoxResult.OK)
            {
                this.Close();
            }
        }


    }
}
