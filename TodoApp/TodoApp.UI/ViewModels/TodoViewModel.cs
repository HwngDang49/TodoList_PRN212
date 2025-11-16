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
        private int? _selectedCategoryId;
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
        }

        public TodoViewModel(NavigationService nav, User currentUser)
        {
            _nav = nav;
            _currentUser = currentUser;

            Todos = new ObservableCollection<Todo>();
            Categories = new ObservableCollection<Category>();
            FilteredTodos = new ObservableCollection<Todo>();

            InitializeCommands();
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
            //AddCategoryCommand = new RelayCommand();
            //ToggleTodoCommand = new RelayCommand();
            //EditTodoCommand = new RelayCommand();
            //DeleteTodoCommand = new RelayCommand();
            //FilterByCategoryCommand = new RelayCommand();
            //AddCategoryCommand = new RelayCommand();
            LogoutCommand = new RelayCommand(o => ExecuteLogout());
        }
        #endregion

        #region Command Handlers
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
