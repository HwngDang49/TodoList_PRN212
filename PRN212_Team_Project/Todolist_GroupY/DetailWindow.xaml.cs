using System.Windows;
using Todolist_GroupY.BLL.Services;
using Todolist_GroupY.DAL.Entities;

namespace Todolist_GroupY
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        private TodoService _todoService = new(); //save
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
                DueDatePicker.Text = EditedOne.DueDate.ToString();
                ReminderDatePicker.Text = EditedOne.ReminderTime.ToString();
                CompletedCheckBox.IsChecked = EditedOne.IsCompleted;
            }
            if (IsView != null)
            {
                Console.WriteLine(IsView.Description);
                //fill data từ selected vào các ô nhập
                TitleTextBox.Text = IsView.Title.ToString();
                DescTextBox.Text = IsView.Description;
                DueDatePicker.Text = IsView.DueDate.ToString();
                ReminderDatePicker.Text = IsView.ReminderTime.ToString();
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
            Todo obj = new Todo() { };
            if (EditedOne != null)
            {
                obj.TodoId = EditedOne.TodoId;
            }
            obj.UserId = LoggedInUser;
            obj.Title = TitleTextBox.Text;
            obj.Description = DescTextBox.Text;
            obj.DueDate = DateTime.Parse(DueDatePicker.Text);
            obj.ReminderTime = DateTime.Parse(ReminderDatePicker.Text);
            obj.IsCompleted = CompletedCheckBox.IsChecked ?? false;


            if (EditedOne != null)
            {
                _todoService.UpdateTodos(obj);
            }
            else
            {
                _todoService.CreateTodos(obj);
            }

            this.Close();
        }
    }
}
