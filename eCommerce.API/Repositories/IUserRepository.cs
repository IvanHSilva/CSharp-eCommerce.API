using eCommerce.API.Models;
using System.Collections.Generic;

namespace eCommerce.API.Repositories {
    public interface IUserRepository {
        public List<User> GetUsers();
        public User GetUser(int id);
        public void InsertUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(int id);
    }
}
