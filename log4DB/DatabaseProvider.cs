using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace log4DB
{
    internal class DatabaseProvider
    {
        ConnectionProvider c = new ConnectionProvider();

        private ArrayList in_parameters = new ArrayList();
        private ArrayList out_parameters = new ArrayList();

        private SqlCommand CreateCommand(string commandText, CommandType commandType)
        {
            return CreateCommand(commandText, commandType, 35);
        }

        private SqlCommand CreateCommand(string commandText, CommandType commandType, int commandTimeout)
        {
            SqlCommand cmd = c.createConnection().CreateCommand();
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
            return cmd;
        }
        private void ProcessInParameters(SqlCommand command)
        {
            foreach (SqlParameter param in in_parameters)
                command.Parameters.Add(param);
        }
        private void ProcessOutParameters(SqlCommand command)
        {
            foreach (SqlParameter param in out_parameters)
                command.Parameters.Add(param);
        }
        private void ClearInParameters()
        {
            in_parameters.Clear();
        }
        private void ClearOutParameters()
        {
            out_parameters.Clear();
        }

        public void AddInParameter(string parameterName, DbType dbType, object value)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.DbType = dbType;
            param.Value = value;
            in_parameters.Add(param);
        }
        public void AddOutParameter(string parameterName, DbType dbType)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.DbType = dbType;
            param.Direction = ParameterDirection.Output;
            out_parameters.Add(param);
        }
        public object GetParameterValue(string parameterName, bool inTrueOutFalse)
        {
            object returnValue = null;
            if (inTrueOutFalse == true)
            {
                foreach (SqlParameter Inparam in in_parameters)
                {
                    if (parameterName == Inparam.ParameterName)
                        returnValue = Inparam.Value;
                }
            }
            else
            {
                foreach (SqlParameter Outparam in out_parameters)
                {
                    if (parameterName == Outparam.ParameterName)
                        returnValue = Outparam.Value;
                }
            }


            out_parameters.Clear();

            if (returnValue != null)
                return returnValue;
            else
                return string.Empty;
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            int returnValue = 0;
            SqlCommand cmd = this.CreateCommand(commandText, commandType);
            this.ProcessInParameters(cmd);

            try
            {
                cmd.Connection.Open();
                returnValue = cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MailLogger m = new MailLogger();
                m.LogException(ex);
            }
            finally
            { cmd.Connection.Close(); in_parameters.Clear(); }


            return returnValue;

        }

        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            object returnValue = null;
            SqlCommand cmd = this.CreateCommand(commandText, commandType);
            this.ProcessInParameters(cmd);
            try
            {
                cmd.Connection.Open();
                returnValue = cmd.ExecuteScalar();
            }
            catch { }
            finally
            { cmd.Connection.Close(); in_parameters.Clear(); }



            if (returnValue != null)
                return returnValue;
            else return "";

        }

        public IDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            IDataReader returnValue = null;
            try
            {
                SqlCommand cmd = this.CreateCommand(commandText, commandType);
                this.ProcessInParameters(cmd);

                cmd.Connection.Open();
                returnValue = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                /* Method kullanıldıktan sonra datareader nesnesi close methodu kapatılmalı. */
            }
            catch { }
            finally
            { in_parameters.Clear(); }

            return returnValue;
        }

        public DataSet ExecuteDataset(string commandText, CommandType commandType)
        {
            return ExecuteDataset(commandText, commandType, string.Empty);
        }

        public DataSet ExecuteDataset(string commandText, CommandType commandType, string datatableName)
        {
            DataSet returnValue = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = this.CreateCommand(commandText, commandType);
            this.ProcessInParameters(da.SelectCommand);
            try
            {
                da.SelectCommand.Connection.Open();

                if (datatableName == string.Empty)
                    da.Fill(returnValue);
                else
                    da.Fill(returnValue, datatableName);
            }
            catch { }
            finally
            { da.SelectCommand.Connection.Close(); in_parameters.Clear(); }

            return returnValue;
        }

        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return ExecuteDataTable(commandText, commandType, string.Empty);
        }

        public DataTable ExecuteDataTable(string commandText, CommandType commandType, string datatableName)
        {
            DataTable returnValue;
            if (datatableName == string.Empty)
                returnValue = ExecuteDataset(commandText, commandType).Tables[0];
            else
                returnValue = ExecuteDataset(commandText, commandType, datatableName).Tables[datatableName];

            return returnValue;
        }

        public void ReturnableExecuteNonQuery(string commandText, CommandType commandType)
        {
            SqlCommand cmd = this.CreateCommand(commandText, commandType);
            this.ProcessInParameters(cmd);
            this.ProcessOutParameters(cmd);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch { }
            finally
            { cmd.Connection.Close(); in_parameters.Clear(); }
        }


    }
}
