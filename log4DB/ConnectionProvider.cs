using System.Configuration;
using System.Data.SqlClient;

namespace log4DB
{
    internal sealed class ConnectionProvider
    {
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["log4DB_Conn"].ConnectionString;
            }
        }

        private static SqlConnection conn = null;

        #region Thread Safe Singleton
        // Thread safe.
        private static object _lockObject = new object();
        #endregion



        /*Singleton pattern*/
        internal SqlConnection createConnection()
        {
            if (conn == null)
            {
                lock (_lockObject)
                {
                    if (conn == null)
                        conn = new SqlConnection(ConnectionString);
                }
            }
            return conn;

        }

    }
}
