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
        private List<Todo> _allTodos = new();
        public MainWindow()
        {
            InitializeComponent();



        }
        public MainWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Lấy toàn bộ Todo của user đang đăng nhập
            _allTodos = _service.GetTodosByUser(UserId);

            //  Hiển thị tất cả lên DataGrid
            FillDataGrid(_allTodos);

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
            IEnumerable<Todo> filtered = _allTodos;

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
    }
}
