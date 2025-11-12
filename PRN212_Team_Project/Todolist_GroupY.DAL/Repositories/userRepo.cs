using Todolist_GroupY.DAL.Entities;

namespace Todolist_GroupY.DAL.Repositories
{
    public class userRepo
    {
        private TodoListNotesDbContext _ctx;
        public User? FindByEmail(string email)
        {
            _ctx = new();
            return _ctx.Users.FirstOrDefault(x => x.Email == email);    
        }

    }
}
