using System;
using System.Collections.Generic;

namespace eCommerce.API.Models {
    public class User {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Gender { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public string Filiation { get; set; }
        public string Situation { get; set; }
        public DateTime RegDate { get; set; }
        
        public Contact Contact { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Department> Departments{ get; set; }

        public User() {}

        public User(int id, string name, string eMail) {
            Id = id;
            Name = name;
            EMail = eMail;
        }

        public User(int id, string name, string eMail, string gender, string rG, string cPF, string filiation, string situation, DateTime regDate) : this(id, name, eMail) {
            Id = id;
            Name = name;
            EMail = eMail;
            Gender = gender;
            RG = rG;
            CPF = cPF;
            Filiation = filiation;
            Situation = situation;
            RegDate = regDate;
        }
    }
}
