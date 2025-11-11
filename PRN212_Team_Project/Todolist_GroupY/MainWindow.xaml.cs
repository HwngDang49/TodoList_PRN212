using System.Windows;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid(_service.GetTodos());
        }

        public void FillDataGrid(List<Todo> bag)
        {
            TodoDataGrid.ItemsSource = null;
            TodoDataGrid.ItemsSource = bag;
        }
    }
}