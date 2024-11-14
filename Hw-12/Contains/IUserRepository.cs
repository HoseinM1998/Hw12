using Hw_12.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw_12.Contains
{
    public interface IUserRepository
    {
        public void Add(User user);
        public List<User> GetAll();
        public User Get(string userName);
        public void Update(User user);
        public void Delete(string userName);

    }

}
