using FilesWebAPI.Data;
using FilesWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace FilesWebAPI.Controllers
{
    [Route("api/users")]
    public class UsersController : ApiController
    {
        private readonly FilesDbContext _context = new FilesDbContext();

        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            var users = new List<User>();

            var connection = _context.GetConnection();
            var command = _context.GetCommand();

            try
            {
                connection.Open();
                command.BindByName = true;
                command.CommandText = "SELECT * FROM USERS";

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User()
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        UserGroupId = Convert.ToInt32(reader["USER_GROUP_ID"]),
                        FirstName = reader["FIRST_NAME"].ToString(),
                        LastName = reader["LAST_NAME"].ToString(),
                        Username = reader["USERNAME"].ToString()
                    });
                }
                reader.Dispose();
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
            finally
            {
                connection.Close();
            }

            return users;
        }

        [HttpGet]
        [Route("api/users/{username}")]
        public User GetUserByUsername(string username)
        {
            var user = new User();

            var connection = _context.GetConnection();
            var command = _context.GetCommand();

            try
            {
                connection.Open();
                command.BindByName = true;
                command.CommandText = $"SELECT * FROM USERS WHERE USERNAME = '{username}'";

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user.Id = Convert.ToInt32(reader["ID"]);
                    user.UserGroupId = Convert.ToInt32(reader["USER_GROUP_ID"]);
                    user.FirstName = reader["FIRST_NAME"].ToString();
                    user.LastName = reader["LAST_NAME"].ToString();
                    user.Username = reader["USERNAME"].ToString();
                }

                reader.Dispose();
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
            finally
            {
                connection.Close();
            }

            return user;
        }

        [HttpPost]
        public void AddUser([FromBody] UserModel user)
        {
            var connection = _context.GetConnection();
            var command = _context.GetCommand();

            try
            {
                connection.Open();
                command.BindByName = true;
                command.CommandText =
                    "INSERT INTO USERS (USER_GROUP_ID, FIRST_NAME, LAST_NAME, USERNAME) " +
                    $"VALUES ((SELECT ID FROM USER_GROUPS WHERE NAME = '{user.UserGroup}'), '{user.FirstName}', '{user.LastName}', '{user.Username}' )";

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
