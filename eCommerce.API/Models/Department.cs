using System.Collections.Generic;

namespace eCommerce.API.Models {
    public class Department {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public Department(int id, string name) {
            Id = id;
            Name = name;
        }
    }
}
