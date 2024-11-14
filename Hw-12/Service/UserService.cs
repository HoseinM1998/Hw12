using Colors.Net.StringColorExtensions;
using Colors.Net;
using Hw_12.Contains;
using Hw_12.Entities;
using Hw_12.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw_12.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService()
        {
            _userRepository = new UserRepository();
        }
        private User _currentUser;

        public void Register(string fullName, string userName, string password)
        {
            try
            {
                bool isSpecial = password.Any(s => (s >= 33 && s <= 47) || s == 64);

                if (password.Length < 5 || password.Length > 10 || !isSpecial)
                {
                    throw new Exception("Password > 4  Char And One Special Character");
                }

                var user1 = _userRepository.Get(userName);
                if (user1 != null)
                {
                    throw new Exception("Username Already Exists");
                }

                var user = new User
                {
                    FullName = fullName,
                    UserName = userName,
                    Password = password,
                    Tasks = new List<Task>()
                };

                _userRepository.Add(user);
            }

            catch (Exception ex)
            {
                throw new Exception($"Error : {ex.Message}", ex);
            }
        }

        public void Login(string username, string password)
        {
            try
            {
                var user = _userRepository.GetAll().FirstOrDefault(u => u.UserName == username && u.Password == password);

                if (user == null)
                {
                    throw new Exception("Invalid username or password.");
                }

                _currentUser = user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error : {ex.Message}", ex);

            }
        }

        public bool Logout()
        {
            _currentUser = null;
            return true;
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }
    }
}
