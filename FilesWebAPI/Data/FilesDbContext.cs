using Oracle.ManagedDataAccess.Client;

namespace FilesWebAPI.Data
{
    public class FilesDbContext
    {
        private OracleConnection _connection;
        private OracleCommand _command;
        private readonly string _connectionString = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = rbsf.w.dedikuoti.lt)(PORT = 1521)))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = xe))); User Id = CANDIDATE_USER3; Password = D0_I_KN0W_Y@;";

        public OracleConnection GetConnection()
        {
            _connection = new OracleConnection(_connectionString);
            return _connection;
        }

        public OracleCommand GetCommand()
        {
            _command = _connection.CreateCommand();
            return _command;
        }
    }
}