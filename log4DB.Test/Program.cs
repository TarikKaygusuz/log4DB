using System;
using System.Data.SqlClient;
using System.Configuration;

namespace log4DB.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SqlException();
            }
            catch (Exception ex)
            {
                LogFactory log = new LogFactory(LogTypes.DatabaseLogger);
                log.LogException(ex);

            }

        }

        static void SqlException()
        {
            string conString = ConfigurationManager.ConnectionStrings["log4DB_Conn"].ToString();
            SqlConnection conn = new SqlConnection(conString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("raiserror('text of exception', 16, 1)", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        static void AppException()
        {
            int y = 0;
            double i = 10 / y;
        }

    }
}
