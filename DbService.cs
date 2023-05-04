using System.Data.SqlClient;
using System.Data;

namespace appDapper2
{
    public class DbService
    {
        private readonly string connectionString;
        private IDbConnection connection;

        public DbService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                }
                return connection;
            }
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }
        }
    }
}
