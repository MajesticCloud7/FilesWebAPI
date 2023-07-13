using FilesWebAPI.Data;
using FilesWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace FilesWebAPI.Controllers
{
    [Route("api/files")]
    public class FilesController : ApiController
    {
        private readonly FilesDbContext _context = new FilesDbContext();

        [HttpGet]
        [Route("api/files/{username}")]
        public IEnumerable<File> GetFilesByUsername(string username)
        {
            var commandText =
                $"SELECT * FROM FILES f " +
                "WHERE EXISTS " +
                "(SELECT 1 FROM USER_FILE_ACCESS ufa " +
                "WHERE ufa.FILE_GROUP_ID = f.FILE_GROUP_ID " +
                "AND ufa.USER_GROUP_ID = " +
                "(SELECT USER_GROUP_ID FROM USERS " +
                $"WHERE USERNAME = '{username}') " +
                ")";
            return GetFiles(commandText);
        }

        [HttpGet]
        [Route("api/files/{fileGroup}/{username}")]
        public IEnumerable<File> GetFilesByGroupAndUsername(string fileGroup, string username)
        {
            var commandText =
                "SELECT * FROM FILES f " +
                "WHERE f.FILE_GROUP_ID = " +
                "(SELECT ID FROM FILE_GROUPS " +
                $"WHERE NAME = '{fileGroup}') " +
                "AND EXISTS " +
                "(SELECT 1 FROM USER_FILE_ACCESS ufa " +
                "WHERE ufa.FILE_GROUP_ID = f.FILE_GROUP_ID " +
                $"AND ufa.USER_GROUP_ID = " +
                "(SELECT USER_GROUP_ID FROM USERS " +
                $"WHERE USERNAME = '{username}') " +
                ")";
            return GetFiles(commandText);
        }

        private IEnumerable<File> GetFiles(string commandText)
        {
            var files = new List<File>();

            var connection = _context.GetConnection();
            var command = _context.GetCommand();

            try
            {
                connection.Open();
                command.BindByName = true;
                command.CommandText = commandText;

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var fileBlob = reader.GetOracleBlob(3).Value;

                    files.Add(new File()
                    {
                        Id = Convert.ToInt32(reader["ID"]),
                        FileGroupId = Convert.ToInt32(reader["FILE_GROUP_ID"]),
                        Name = reader["NAME"].ToString(),
                        FileBlob = fileBlob
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

            return files;
        }
    }
}
