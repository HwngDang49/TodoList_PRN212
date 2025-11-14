using Todolist_GroupY.DAL.Entities;
using Todolist_GroupY.DAL.Repositories;

namespace Todolist_GroupY.BLL.Services
{
    public class UserService
    {
        private userRepo _repo = new();
        public User? Authenticate(string email)
        {
            return _repo.FindByEmail(email);
        }
        public void RegisterUser(User user)
        {
            // Hàm này gọi hàm Create của Repo
            _repo.Create(user);
        }
    }
}
