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
                    user.RG = reader.GetString("RG");
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
                command.CommandText = $"SELECT *, c.Id ContId, e.Id EndId, ud.Id UDId, d.Id DepId, d.Nome DepNome FROM Usuarios u ";
                command.CommandText += "LEFT JOIN Contatos c ON c.UsuId = u.Id ";
                command.CommandText += "LEFT JOIN Enderecos e ON e.UsuId = u.Id ";
                command.CommandText += "LEFT JOIN UsuDeptos ud ON ud.UsuId = u.Id ";
                command.CommandText += "LEFT JOIN Departamentos d ON ud.DeptoId = d.Id ";
                command.CommandText += "WHERE u.Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                command.Connection = (SqlConnection)_connection;
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Dictionary<int, User> users = new Dictionary<int, User>();

                while (reader.Read()) {
                    User user = new User();
                    if (!(users.ContainsKey(reader.GetInt32("Id")))) {
                        user.Id = reader.GetInt32("Id");
                        user.Name = reader.GetString("Nome");
                        user.EMail = reader.GetString("Email");
                        user.Gender = reader.GetString("Sexo");
                        user.RG = reader.GetString("RG");
                        user.CPF = reader.GetString("CPF");
                        user.Filiation = reader.GetString("Filiacao");
                        user.Situation = reader.GetString("Situacao");
                        user.RegDate = reader.GetDateTime("DataCad");

                        Contact contact = new Contact();
                        contact.Id = reader.GetInt32("ContId");
                        contact.UserId = user.Id;
                        contact.Phone = reader.GetString("Telefone");
                        contact.CellPhone = reader.GetString("Celular");
                        user.Contact = contact;

                        users.Add(user.Id, user);
                    } else {
                        user = users[reader.GetInt32("Id")];
                    }
                    
                    Address address = new Address();
                    address.Id = reader.GetInt32("EndId");
                    address.UserId = user.Id;
                    address.Description = reader.GetString("Descricao");
                    address.Street = reader.GetString("Endereco");
                    address.Number = reader.GetString("Numero");
                    address.Comp = reader.GetString("Complemento");
                    address.District = reader.GetString("Bairro");
                    address.City = reader.GetString("Cidade");
                    address.State = reader.GetString("Estado");
                    address.ZipCode = reader.GetString("CEP");
                    
                    user.Addresses = (user.Addresses == null) ? new List<Address>() : user.Addresses;
                    if (user.Addresses.FirstOrDefault(a => a.Id == address.Id) == null) {
                        user.Addresses.Add(address);
                    }

                    Department department = new Department();
                    department.Id = reader.GetInt32("DepId");
                    department.Name = reader.GetString("DepNome");

                    user.Departments = (user.Departments == null) ? new List<Department>() : user.Departments;
                    if (user.Departments.FirstOrDefault(d => d.Id == department.Id) == null) {
                        user.Departments.Add(department);
                    }
                }
                return users[users.Keys.First()];
            } catch (Exception e) {
                string error = e.Message;
                return null;
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
                command.CommandText += "SELECT CAST(scope_identity() AS int)";
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
                user.Id = (int)command.ExecuteScalar();
            } catch (Exception e) {
                string error = e.Message;
            } finally {
                _connection.Close();
            }
        }

        public void UpdateUser(User user) {
            try {
                SqlCommand command = new SqlCommand();
                command.CommandText = "UPDATE Usuarios SET Nome = @Nome, EMail = @EMail, Sexo = @Sexo, RG = @RG, CPF = @CPF, ";
                command.CommandText += "Filiacao = @Filiacao, Situacao = @Situacao, DataCad = @DataCad WHERE Id = @Id";
                command.Connection = (SqlConnection)_connection;
                command.Parameters.AddWithValue("@Nome", user.Name);
                command.Parameters.AddWithValue("@EMail", user.EMail);
                command.Parameters.AddWithValue("@Sexo", user.Gender);
                command.Parameters.AddWithValue("@RG", user.RG);
                command.Parameters.AddWithValue("@CPF", user.CPF);
                command.Parameters.AddWithValue("@Filiacao", user.Filiation);
                command.Parameters.AddWithValue("@Situacao", user.Situation);
                command.Parameters.AddWithValue("@DataCad", user.RegDate);
                command.Parameters.AddWithValue("@Id", user.Id);
                _connection.Open();
                command.ExecuteNonQuery();
            } catch (Exception e) {
                string error = e.Message;
            } finally {
                _connection.Close();
            }
        }

        public void DeleteUser(int id) {
            try {
                SqlCommand command = new SqlCommand();
                command.CommandText = "DELETE FROM Usuarios WHERE Id = @Id";
                command.Connection = (SqlConnection)_connection;
                command.Parameters.AddWithValue("@Id", id);
                _connection.Open();
                command.ExecuteNonQuery();
            } catch (Exception e) {
                string error = e.Message;
            } finally {
                _connection.Close();
            }
        }
    }
}
