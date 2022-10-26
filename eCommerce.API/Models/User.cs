namespace eCommerce.API.Models {
    public class User {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }

        public User(int id, string name, string eMail) {
            Id = id;
            Name = name;
            EMail = eMail;
        }
    }
}
