using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories;

namespace TodoApp.BLL.Services
{
    public class UserService
    {
        private UserRepository _repo = new();

        /*
        *  SignUp 
        */
        public void SignUp(User user)
        {
            _repo.Create(user);
        }

        /*
        *  Login 
        */
        public User? Login(string email, string password)
        {
            return _repo.GetByEmailPassword(email, password);
        }
    }
}
