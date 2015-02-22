using System;
using System.Data.SqlClient;
using System.Text;

namespace log4DB
{
    internal class Logger
    {
        public Logger()
        {

        }
        public string GetSqlError(string message, SqlErrorCollection errors)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(message);
            foreach (SqlError error in errors)
            {
                sb.Append(Environment.NewLine);
                sb.Append(" Procedure : ");
                sb.Append(error.Procedure);

                sb.Append(Environment.NewLine);
                sb.Append(" Satır Numarası : ");
                sb.Append(error.LineNumber);

                sb.Append(Environment.NewLine);
                sb.Append(" Önem Derecesi : ");
                sb.Append(error.Class);

                sb.Append(Environment.NewLine);
                sb.Append(" Message : ");
                sb.Append(error.Message);

                sb.Append(Environment.NewLine);
                sb.Append(" Server : ");
                sb.Append(error.Server);

                sb.Append(Environment.NewLine);
                sb.Append(" State : ");
                sb.Append(error.State);

                sb.Append(Environment.NewLine);
                sb.Append(" Time : ");
                sb.Append(DateTime.Now.ToString());
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);

            }
            return sb.ToString();
        }

    }

}
