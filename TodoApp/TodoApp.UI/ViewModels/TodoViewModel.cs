using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TodoApp.BLL.Services;
using TodoApp.DAL;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories;
using TodoApp.UI.Command;
using TodoApp.UI.Dialogs;
using TodoApp.UI.Services;

namespace TodoApp.UI.ViewModels
{
    public class TodoViewModel : BaseViewModel
    {
        private readonly NavigationService _nav;
        private readonly TodoService _todoService;
        private readonly CategoryService _categoryService;

        private User _currentUser;
        private string _newTodoTitle;
        private string _currentCategoryName = "All Tasks";
        private Guid? _selectedCategoryId;
        private ObservableCollection<Todo> _todoList;
        private ObservableCollection<Todo> _filteredTodos;
        private ObservableCollection<Category> _categories;

        public TodoViewModel(NavigationService nav, User currentUser, TodoService todoService, CategoryService categoryService)
        {
            _nav = nav;
            _currentUser = currentUser;
            _todoService = todoService;
            _categoryService = categoryService;

            Todos = new ObservableCollection<Todo>();
            Categories = new ObservableCollection<Category>();
            FilteredTodos = new ObservableCollection<Todo>();

            InitializeCommands();

            // Check if user is not null before loading data
            if (_currentUser != null)
            {
                LoadData();
            }
            else
            {
                MessageBox.Show("Error: User not found. Please login again.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Properties
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public string NewTodoTitle
        {
            get => _newTodoTitle;
            set
            {
                _newTodoTitle = value;
                OnPropertyChanged();
            }
        }

        public string CurrentCategoryName
        {
            get => _currentCategoryName;
            set
            {
                _currentCategoryName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Todo> Todos
        {
            get => _todoList;
            set
            {
                _todoList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Todo> FilteredTodos
        {
            get => _filteredTodos;
            set
            {
                _filteredTodos = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddTodoCommand { get; private set; }
        public RelayCommand ToggleTodoCommand { get; private set; }
        public RelayCommand EditTodoCommand { get; private set; }
        public RelayCommand DeleteTodoCommand { get; private set; }
        public RelayCommand FilterByCategoryCommand { get; private set; }
        public RelayCommand AddCategoryCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }

        private void InitializeCommands()
        {
            AddTodoCommand = new RelayCommand(o => ExecuteAddTodo(), o => CanExecuteAddTodo());
            ToggleTodoCommand = new RelayCommand(o => ExecuteToggleTodo(o as Todo));
            EditTodoCommand = new RelayCommand(o => ExecuteEditTodo(o as Todo));
            DeleteTodoCommand = new RelayCommand(o => ExecuteDeleteTodo(o as Todo));
            FilterByCategoryCommand = new RelayCommand(o => ExecuteFilterByCategory(o));
            AddCategoryCommand = new RelayCommand(o => ExecuteAddCategory());
            LogoutCommand = new RelayCommand(o => ExecuteLogout());
        }
        #endregion

        #region Command Handlers
        /*
        *  Load Data
        */
        private void LoadData() 
        {
            try
            {
                // Load data for current user
                var todos = _todoService.GetTodos(_currentUser.UserId);
                Todos.Clear();
                foreach (var todo in todos)
                {
                    // Load category name if exists
                    if (todo.CategoryId.HasValue)
                    {
                        var category = _categoryService.GetCategoryById(todo.CategoryId.Value);
                        todo.CategoryName = category?.Name;
                    }
                    Todos.Add(todo);
                }

                // Load categories for current user
                var categories = _categoryService.GetCategories(_currentUser.UserId);
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                // Show all todos initially
                FilterTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        *  Can execute Add Todo
        */
        private bool CanExecuteAddTodo()
        {
            return !string.IsNullOrWhiteSpace(NewTodoTitle);
        }

        /*
        *  Execute Add Todo
        */
        private void ExecuteAddTodo()
        {
            try
            {
                var newTodo = new Todo()
                {
                    TodoId = Guid.NewGuid(),
                    UserId = _currentUser.UserId,
                    Title = NewTodoTitle.Trim(),
                    Description = string.Empty,
                    IsCompleted = false,
                    CategoryId = _selectedCategoryId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                // Add Todo to database
                _todoService.CreateTodo(newTodo);

                Todos.Add(newTodo);
                FilterTodos();

                // Clear Input
                NewTodoTitle = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding todo: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        *  Execute Add Category
        */
        private void ExecuteAddCategory()
        {
            try
            {
                var dialog = new AddCategoryDialog();
                if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.CategoryName))
                {   
                    var newCategory = new Category
                    {
                        CategoryId = Guid.NewGuid(),
                        UserId = _currentUser.UserId,
                        Name = dialog.CategoryName.Trim(),
                        CreatedAt = DateTime.Now
                    };

                    // Debug output
                    System.Diagnostics.Debug.WriteLine($"Creating category name: {newCategory.Name}");
                    System.Diagnostics.Debug.WriteLine($"Creating userId: {newCategory.UserId}");
                    System.Diagnostics.Debug.WriteLine($"Creating Category Id: {newCategory.CategoryId}");

                    _categoryService.CreateCategory(newCategory);
                    Categories.Add(newCategory);
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Database Error Details:");
                sb.AppendLine(ex.Message);

                if (ex.InnerException != null)
                {
                    sb.AppendLine($"\nInner Exception: {ex.InnerException.Message}");

                    if (ex.InnerException.InnerException != null)
                    {
                        sb.AppendLine($"\nDatabase Error: {ex.InnerException.InnerException.Message}");
                    }
                }

                System.Diagnostics.Debug.WriteLine(sb.ToString());
                MessageBox.Show(sb.ToString(), "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        *  Execute Toggle Todo
        */
        private void ExecuteToggleTodo(Todo todo)
        {
            if (todo == null) return;

            try
            {
                todo.IsCompleted = !todo.IsCompleted;
                todo.UpdatedAt = DateTime.Now;

                _todoService.UpdateTodo(todo);

                // Trigger UI update
                OnPropertyChanged(nameof(FilteredTodos));
            }
            catch (Exception ex)
            {
                // Revert on error
                todo.IsCompleted = !todo.IsCompleted;
                MessageBox.Show($"Error updating todo: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        *  Execute Edit Todo
        */
        private void ExecuteEditTodo(Todo todo)
        {
            if (todo == null) return;

            try
            {
                var editDialog = new EditTodoDialog(todo, Categories.ToList());
                if (editDialog.ShowDialog() == true)
                {
                    var editedTodo = editDialog.EditedTodo;

                    // Update the todo in the service
                    _todoService.UpdateTodo(editedTodo);

                    // Update the todo in the collection
                    var index = Todos.IndexOf(todo);
                    if (index >= 0)
                    {
                        Todos[index] = editedTodo;
                    }

                    FilterTodos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating todo: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        *  Execute Delete Todo
        */
        private void ExecuteDeleteTodo(Todo todo)
        {
            if (todo == null) return;

            try
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{todo.Title}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _todoService.DeleteTodo(todo);
                    Todos.Remove(todo);
                    FilterTodos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting todo: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*
        *  Execute Filter By Category
        */
        private void ExecuteFilterByCategory(object parameter)
        {
            if (parameter is string && parameter.ToString() == "All")
            {
                _selectedCategoryId = null;
                CurrentCategoryName = "All Tasks";
            }
            else if (parameter is Guid categoryId)
            {
                _selectedCategoryId = categoryId;
                var category = Categories.FirstOrDefault(c => c.CategoryId == categoryId);
                CurrentCategoryName = category?.Name ?? "Unknown Category";
            }

            FilterTodos();
        }

        /*
        *  Filter Todos
        */
        private void FilterTodos()
        {
            FilteredTodos.Clear();

            var filtered = _selectedCategoryId.HasValue
                ? Todos.Where(t => t.CategoryId == _selectedCategoryId.Value)
                : Todos;

            // Order by: incomplete first, then by creation date (newest first)
            foreach (var todo in filtered.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.CreatedAt))
            {
                FilteredTodos.Add(todo);
            }
        }

        /*
        *  Logout
        */
        private void ExecuteLogout()
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var db = new TodoDbContext();
                var userRepo = new UserRepository(db);
                _nav.NavigateTo(new LoginViewModel(_nav, new UserService(userRepo)));
            }
        }
        #endregion
    }
}
