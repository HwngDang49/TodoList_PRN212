using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Todolist_GroupY.BLL.Services;
using Todolist_GroupY.DAL.Entities;

namespace Todolist_GroupY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TodoService _service = new();
        public int UserId { get; set; }
        public MainWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //  Hiển thị tất cả lên DataGrid
            FillDataGrid(_service.GetTodosByUser(UserId));

            // Hiển thị ngày hôm nay
            TodayLabel.Content = DateTime.Now.ToString("dd/MM/yyyy");
        }

        public void FillDataGrid(List<Todo> bag)
        {
            TodoDataGrid.ItemsSource = null;
            TodoDataGrid.ItemsSource = bag;
        }

        private void ApplyFilters()
        {
            IEnumerable<Todo> filtered = _service.GetTodosByUser(UserId);

            // 1 Lọc theo ngày
            if (FilterDatePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = FilterDatePicker.SelectedDate.Value.Date;
                filtered = filtered.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == selectedDate);
            }

            //  Lọc theo hoàn thành
            if (ShowCompletedCheckBox.IsChecked == true)
            {
                filtered = filtered.Where(t => t.IsCompleted ?? false);
            }

            //  Lọc theo ô tìm kiếm (tiêu đề)
            string keyword = SearchTextBox.Text.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filtered = filtered.Where(t => t.Title != null && t.Title.ToLower().Contains(keyword));
            }

            FillDataGrid(filtered.ToList());
        }

        private void FilterDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ShowCompletedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ShowCompletedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Todo? selected = TodoDataGrid.SelectedItem as Todo;
            if (selected == null)
            {
                MessageBox.Show("Please select a row before deleting", "Select one", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult answer = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
            {
                return;
            }

            _service.DeleteTodos(selected);

            FillDataGrid(_service.GetTodosByUser(UserId));
        }

        private void ViewDetailButton_Click_1(object sender, RoutedEventArgs e)
        {
            Todo? selected = TodoDataGrid.SelectedItem as Todo;
            if (selected == null)
            {
                MessageBox.Show("Please select a row before deleting", "Select one", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DetailWindow detail = new();
            detail.IsView = selected;

            detail.ShowDialog();

            FillDataGrid(_service.GetTodosByUser(UserId));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            DetailWindow detail = new();
            detail.LoggedInUser = UserId;

            detail.ShowDialog();
            FillDataGrid(_service.GetTodosByUser(UserId));
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Todo? selected = TodoDataGrid.SelectedItem as Todo;
            Console.WriteLine(selected);
            if (selected == null)
            {
                MessageBox.Show("Please select a row before deleting", "Select one", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DetailWindow detail = new();

            detail.EditedOne = selected;
            detail.LoggedInUser = UserId;

            detail.ShowDialog();
            FillDataGrid(_service.GetTodosByUser(UserId));
        }
        // Override phương thức OnClosing để ngăn cửa sổ chính đóng lại
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true; // Ngăn không cho cửa sổ đóng
            this.Hide(); // Thay vào đó, ẩn cửa sổ
        }
    }
}
