using Shared.Serialization;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Server.Logic
{
    public class DatabaseInfo
    {
        private static SqlConnection _connection;
        static DatabaseInfo()
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }
        /// <summary>
        /// Connection to database
        /// </summary>
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        /// <summary>
        /// Find user in database with such login 
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Id user</returns>
        public static int FindUser(string login)
        {
            string sqlExpression = $"select [dbo].FindByLogin('{login}')";
            using (SqlCommand command = new SqlCommand(sqlExpression, _connection))
            {
                using var reader = command.ExecuteReader();
                if (reader.Read() && reader.IsDBNull(0))
                {
                    return -1;
                }
                return reader.GetInt32(0);
            }
        }
        
        public static void AddUser(string login, string password)
        {
            string sqlExpression = "sp_InsertUser";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(sqlExpression, _connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter LoginParam = new SqlParameter
                {
                    ParameterName = "@Login",
                    Value = login
                };
                command.Parameters.Add(LoginParam);

                SqlParameter PasswordParam = new SqlParameter
                {
                    ParameterName = "@Password",
                    Value = password
                };
                command.Parameters.Add(PasswordParam);
                var result = command.ExecuteScalar();
            }
        }
        /// <summary>
        /// Creating struct userinfo
        /// </summary>
        /// <returns>Linkedlist of users</returns>
        public static LinkedList<UserInfo> GetUsersInfo()
        {
            var listUsers = new LinkedList<UserInfo>();
            string sqlExpression = "sp_GetUsers";
            using var command = new SqlCommand(sqlExpression, _connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string login = reader.GetString(1);
                    string password = reader.GetString(2);
                    string email = reader.GetString(3);
                    var User = new UserInfo
                    {
                        ID = id,
                        Login = login,
                        Password = password,
                        Email = email
                    };
                    listUsers.AddLast(User);
                }
            }

            return listUsers;
        }
    }
}

