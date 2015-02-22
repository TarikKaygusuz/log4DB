using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace log4DB
{
    internal class DatabaseLogger : Logger, ILogger
    {

        private ExceptionManaging exmanage = null;
        DatabaseProvider db = new DatabaseProvider();
        private StringBuilder sb = null;


        private void CreateTables()
        {
            sb = new StringBuilder();

            #region SqlLogs Tablosu
            sb.Append(" if object_id('log4DB_SqlLogs', 'U') is null ");
            sb.Append(" create table log4DB_SqlLogs ");
            sb.Append(" ( ");
            sb.Append(" SqlLogID int not null identity(1,1) primary key clustered, ");
            sb.Append(" ApplicationName nvarchar(250) null, ");
            sb.Append(" [Procedure] nvarchar(250) null, ");
            sb.Append(" SatirNumarasi nvarchar(250) null, ");
            sb.Append(" OnemDerecesi nvarchar(250) null, ");
            sb.Append(" [Message]  nvarchar(4000) null, ");
            sb.Append(" [Server] nvarchar(250) null, ");
            sb.Append(" [Time] nvarchar(20) null ");
            sb.Append(" ) ");
            #endregion

            #region Application Logs Tablosu
            sb.Append(" if object_id('log4DB_Applicationlogs', 'U') is null ");
            sb.Append(" create table log4DB_Applicationlogs ");
            sb.Append(" ( ");
            sb.Append(" ApplicationLogID int not null identity(1,1) primary key clustered, ");
            sb.Append(" ApplicationName nvarchar(250) null, ");
            sb.Append(" [Message]  nvarchar(1000) null, ");
            sb.Append(" Source nvarchar(4000) null, ");
            sb.Append(" StackTrace nvarchar(4000) null, ");
            sb.Append(" InnerException nvarchar(4000) null, ");
            sb.Append(" [Time] nvarchar(20) null ");

            sb.Append(" ) ");
            #endregion

            // Tüm tabloları (yoksa) yaratır.
            db.ExecuteNonQuery(sb.ToString(), CommandType.Text);

        }
        private void CreateProcs()
        {
            #region Sql Logs Tutacak Proc
            sb = new StringBuilder();
            sb.Append(" IF NOT EXISTS (SELECT name ");
            sb.Append(" FROM sysobjects ");
            sb.Append(" WHERE name = N'log4DB_insertTo_SqlLogs' ");
            sb.Append(" AND    type = 'P') ");
            sb.Append(" EXEC(' ");
            sb.Append(" create Proc log4DB_insertTo_SqlLogs ");
            sb.Append(" ( ");
            sb.Append(" @ApplicationName nvarchar(500), ");
            sb.Append(" @Procedure nvarchar(MAX), ");
            sb.Append(" @SatirNumarasi nvarchar(MAX), ");
            sb.Append(" @OnemDerecesi nvarchar(MAX), ");
            sb.Append(" @Message nvarchar(MAX), ");
            sb.Append(" @Server nvarchar(MAX), ");
            sb.Append(" @Time nvarchar (20)");
            sb.Append(" ) ");
            sb.Append(" AS ");
            sb.Append(" SET NOCOUNT ON ");
            sb.Append(" insert into log4DB_SqlLogs values (@ApplicationName,@Procedure,@SatirNumarasi,@OnemDerecesi,@Message,@Server,@Time); ");
            sb.Append("')");
            #endregion

            #region Application Logs Tutacak Proc
            sb.Append(" IF NOT EXISTS (SELECT name ");
            sb.Append(" FROM   sysobjects ");
            sb.Append(" WHERE  name = N'log4DB_insertTo_Applicationlogs' ");
            sb.Append(" AND    type = 'P') ");
            sb.Append(" EXEC(' ");
            sb.Append(" Create Proc log4DB_insertTo_Applicationlogs ");
            sb.Append(" @ApplicationName nvarchar (500), ");
            sb.Append(" @Message nvarchar(MAX), ");
            sb.Append(" @Source  nvarchar(MAX), ");
            sb.Append(" @StackTrace nvarchar(MAX), ");
            sb.Append(" @InnerException nvarchar(MAX), ");
            sb.Append(" @Time nvarchar (20)");

            sb.Append(" AS ");
            sb.Append(" Begin ");
            sb.Append(" SET NOCOUNT ON ");
            sb.Append(" insert into log4DB_Applicationlogs values (@ApplicationName,@Message,@Source,@StackTrace,@InnerException,@Time) ");
            sb.Append(" End ");
            sb.Append("')");
            #endregion

            db.ExecuteNonQuery(sb.ToString(), CommandType.Text);

        }

        /// <summary>
        /// Constractor'in içinde Initialize edilecek methodlar,ilk çalışmadan sonra command'lenebilir.Tablo yoksa yaratır.
        /// </summary>
        public DatabaseLogger()
        {
            CreateTables();
            CreateProcs();
        }

        private void LogSqlErrors(string message, SqlErrorCollection errors)
        {
            foreach (SqlError error in errors)
            {
                db.AddInParameter("@ApplicationName", DbType.String, AppDomain.CurrentDomain.FriendlyName);
                db.AddInParameter("@Procedure", DbType.String, error.Procedure);
                db.AddInParameter("@SatirNumarasi", DbType.String, error.LineNumber);
                db.AddInParameter("@OnemDerecesi", DbType.String, error.Class);
                db.AddInParameter("@Message", DbType.String, message);
                db.AddInParameter("@Server", DbType.String, error.Server);
                db.AddInParameter("@Time", DbType.String, DateTime.Now.ToString());
                db.ExecuteNonQuery("log4DB_insertTo_SqlLogs", CommandType.StoredProcedure);
            }
        }
        private void LogApplicationError(Exception ex)
        {
            db.AddInParameter("@ApplicationName", DbType.String, AppDomain.CurrentDomain.FriendlyName);
            db.AddInParameter("@Message", DbType.String, ex.Message);
            db.AddInParameter("@Source", DbType.String, ex.Source);
            db.AddInParameter("@StackTrace", DbType.String, ex.StackTrace);
            db.AddInParameter("@InnerException", DbType.String, ex.InnerException == null ? string.Empty : ex.InnerException.ToString());
            db.AddInParameter("@Time", DbType.String, DateTime.Now.ToString());
            db.ExecuteNonQuery("log4DB_insertTo_Applicationlogs", CommandType.StoredProcedure);

        }

        public void LogException(Exception ex)
        {
            try
            {
                #region LogException

                SqlException sqlEx = ex as SqlException;
                if (sqlEx != null)
                {
                    LogSqlErrors(ex.Message + ex.StackTrace, sqlEx.Errors);

                }
                else
                {
                    LogApplicationError(ex);

                }
                #endregion
            }
            catch (Exception UnexpecEx)
            {
                exmanage = new ExceptionManaging();
                exmanage.DatabaseLogUnexpedtedError(UnexpecEx);
            }
        }
    }
}
