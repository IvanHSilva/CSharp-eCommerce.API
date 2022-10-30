using eCommerce.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

/* ADO Objects
 * Connection -> Faz a conexão com o banco
 * Command -> executa os comandos sem retorno INSERT, UPDATE, DELETE
 * DataReader -> utiliza arquitetura conectada para usar o comando SELECT
 * DataAdapter -> utiliza arquitetura desconectada (cache) para usar o comando SELECT
 */

namespace eCommerce.API.Repositories {
    public class UserRepository : IUserRepository {

        private IDbConnection _connection;

        public UserRepository() {
            _connection = new SqlConnection("Data Source=SERVIDOR\\SQLSERVER;Initial Catalog=eCommerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public List<User> GetUsers() {
            List<User> users = new List<User>();
            try {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Usuarios";
                command.Connection = (SqlConnection)_connection;
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                //Dapper, EF, NHibernate (ORM)
                while (reader.Read()) {
                    User user = new User();
                    //user.Id = (int)reader["Id"];
                    user.Id = reader.GetInt32("Id");
                    user.Name = reader.GetString("Nome");
                    user.EMail = reader.GetString("Email");
                    user.Gender = reader.GetString("Sexo");
                    user.RG= reader.GetString("RG");
                    user.CPF = reader.GetString("CPF");
                    user.Filiation = reader.GetString("Filiacao");
                    user.Situation = reader.GetString("Situacao");
                    user.RegDate = reader.GetDateTime("DataCad");
                    users.Add(user);
                }
            } catch (Exception e) {
                string error = e.Message;
            } finally {
                _connection.Close();
            }
            return users;
        }

        public User GetUser(int id) {
            try {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Usuarios WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                command.Connection = (SqlConnection)_connection;
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    User user = new User();
                    user.Id = reader.GetInt32("Id");
                    user.Name = reader.GetString("Nome");
                    user.EMail = reader.GetString("Email");
                    user.Gender = reader.GetString("Sexo");
                    user.RG = reader.GetString("RG");
                    user.CPF = reader.GetString("CPF");
                    user.Filiation = reader.GetString("Filiacao");
                    user.Situation = reader.GetString("Situacao");
                    user.RegDate = reader.GetDateTime("DataCad");
                    return user;
                }
            } catch (Exception e) {
                string error = e.Message;
            } finally {
                _connection.Close();
            }
            return null;
        }

        public void InsertUser(User user) {
            try {
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT INTO Usuarios (Nome, EMail, Sexo, RG, CPF, Filiacao, Situacao, DataCad) ";
                command.CommandText += "VALUES (@Nome, @EMail, @Sexo, @RG, @CPF, @Filiacao, @Situacao, @DataCad);";
                command.Connection = (SqlConnection)_connection;
                command.Parameters.AddWithValue("@Nome", user.Name);
                command.Parameters.AddWithValue("@EMail", user.EMail);
                command.Parameters.AddWithValue("@Sexo", user.Gender);
                command.Parameters.AddWithValue("@RG", user.RG);
                command.Parameters.AddWithValue("@CPF", user.CPF);
                command.Parameters.AddWithValue("@Filiacao", user.Filiation);
                command.Parameters.AddWithValue("@Situacao", user.Situation);
                command.Parameters.AddWithValue("@DataCad", user.RegDate);
                _connection.Open();
                command.ExecuteNonQuery();
            } catch (Exception e) {
                string error = e.Message;
            } finally {
                _connection.Close();
            }
        }

        public void UpdateUser(User user) {
            _dbUsers.Remove(_dbUsers.FirstOrDefault(u => u.Id == user.Id));
            _dbUsers.Add(user);
        }

        public void DeleteUser(int id) {
            _dbUsers.Remove(_dbUsers.FirstOrDefault(u => u.Id == id));
        }

        private List<User> _dbUsers = new List<User>() {
            new User(1, "José Rodrigues", "jrodrigues@gmail.com"),
            new User(2, "Maria Teresa", "marite@gmail.com"),
            new User(3, "Ronaldo Amaral", "roamaral@gmail.com"),
            new User(4, "Ana Clarice Mendes", "anacmendes@gmail.com"),
            new User(5, "Xavier Oliveria", "xaoliver@gmail.com"),
        };
    }
}
