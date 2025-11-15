using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.BLL.Services;
using TodoApp.UI.Services;

namespace TodoApp.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public NavigationService NavService { get; set; }

        public MainViewModel()
        {
            NavService = new NavigationService();
            NavService.Navigate = (vm) => CurrentViewModel = vm;

            var userService = new UserService();

            CurrentViewModel = new LoginViewModel(NavService, userService); 
        }
    }
}
