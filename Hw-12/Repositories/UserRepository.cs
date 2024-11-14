using Hw_12.Configuration;
using Hw_12.Contains;
using Hw_12.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw_12.Repositories
{
    public class UserRepository : IUserRepository
    {

        TaskDbContext _context = new TaskDbContext();

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Delete(string userName)
        {
            var user = Get(userName);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User Get(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public void Update(User user)
        {
            var updateUser = Get(user.UserName);
            if (updateUser != null)
            {
                updateUser.FullName = user.FullName;
                updateUser.UserName = user.UserName;
                updateUser.Password = user.Password;
                updateUser.Tasks = user.Tasks;
                _context.SaveChanges();
            }
        }
    }
}



