using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TodoApp.BLL.Services;
using TodoApp.UI.Command;
using TodoApp.UI.Services;

namespace TodoApp.UI.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly NavigationService _nav;
        private readonly UserService _userService;

        private string _email;
        public string Email
        {
            get => _email;
            set 
            { 
                _email = value; 
                OnPropertyChanged(); 
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set 
            { 
                _password = value; 
                OnPropertyChanged(); 
            }
        }

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand GoToSignUpCommand { get; set; }

        public LoginViewModel(NavigationService nav, UserService userService)
        {
            _nav = nav;
            _userService = userService;

            LoginCommand = new RelayCommand(o => Login());
            GoToSignUpCommand = new RelayCommand(o => _nav.NavigateTo(new SignUpViewModel(_nav, _userService)));

        }

        private void Login()
        {
            var user = _userService.Login(Email, Password);

            if (user != null)
            {
                _nav.NavigateTo(new TodoViewModel());
            }
            else
            {
                MessageBox.Show("Invalid email or password");
            }
        }
    }
}
