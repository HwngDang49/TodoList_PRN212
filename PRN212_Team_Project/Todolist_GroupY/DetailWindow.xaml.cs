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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Todo obj = new Todo() { };
                if (EditedOne != null)
                {
                    obj.TodoId = EditedOne.TodoId;
                }
                obj.UserId = LoggedInUser;
                obj.Title = TitleTextBox.Text;
                obj.Description = DescTextBox.Text;
                obj.DueDate = DueDatePicker.Value;
                obj.ReminderTime = ReminderDatePicker.Value;
                obj.IsCompleted = CompletedCheckBox.IsChecked ?? false;

                // Xác định loại thay đổi
                TodoChangeType changeType;

                if (EditedOne != null)
                {
                    // Update existing todo
                    _todoService.UpdateTodos(obj);
                    changeType = TodoChangeType.Updated;
                }
                else
                {
                    // Create new todo
                    _todoService.CreateTodos(obj);
                    changeType = TodoChangeType.Created;
                }

                // Publish event để MainWindow auto-refresh
                _eventBus.PublishTodoChanged(changeType, LoggedInUser, obj.TodoId);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
