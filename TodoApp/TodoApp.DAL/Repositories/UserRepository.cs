using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private TodoDbContext _db;

        /*
        *  Create  
        */
        public void Create(User entity)
        {
            _db = new();
            _db.Users.Add(entity);
            _db.SaveChanges();
        }

        /*
        *  Delete  
        */
        public void Delete(Guid id)
        {
            _db = new();
            _db.Remove(id);
            _db.SaveChanges();
        }

        /*
        *  Delete All  
        */
        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        /*
        *  Get All  
        */
        public List<User> GetAll()
        {
            _db = new();
            return _db.Users.ToList();
        }

        public User? GetById(Guid id)
        {
            _db = new();
            return _db.Users.FirstOrDefault(x => x.UserId == id);
        }

        /*
        *  Get By Email  
        */
        public User? GetByEmail(string email)
        {
            _db = new();
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

        /*
        *  Get By Email/Password
        */
        public User? GetByEmailPassword(string email, string password)
        {
            _db = new();
            return _db.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
        }

        /*
        *  Update 
        */
        public void Update(User user)
        {
            _db = new();
            _db.Users.Update(user);
            _db.SaveChanges();
        }
    }
}
