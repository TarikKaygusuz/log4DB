using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace log4DB
{
    internal class MailLogger : Logger, ILogger
    {
        string GONDEREN_ADI = "log4DB";
        string GONDERILECEK_ADI = "log4DB";
        string GONDEREN_EMAIL_ADRESS = ConfigurationManager.AppSettings["mailFrom"].ToString();
        string GONDERILECEK_EMAIL_ADRESS = ConfigurationManager.AppSettings["mailTo"].ToString();
        string USER_NAME = ConfigurationManager.AppSettings["smtpUser"].ToString();
        string PASSWORD = ConfigurationManager.AppSettings["smtpPass"].ToString();
        string SMTP_SERVER = ConfigurationManager.AppSettings["smtpHost"].ToString();
        string SMTP_PORT = ConfigurationManager.AppSettings["smtpPort"].ToString();

        const SmtpFormat METIN_FORMATI = SmtpFormat.Text;


        private SmtpMail m = null;
        private SmtpServerSettings settings = null;

        private void LogSqlErrors(string message, SqlErrorCollection errors)
        {
            #region LogSqlErros
            m = new SmtpMail(GONDEREN_EMAIL_ADRESS, GONDEREN_ADI, GONDERILECEK_EMAIL_ADRESS, GONDERILECEK_ADI, "Sql Server Error", METIN_FORMATI, GetSqlError(message, errors));
            settings = new SmtpServerSettings(SMTP_SERVER, USER_NAME, PASSWORD, true, Int32.Parse(SMTP_PORT));
            m.MailGonder(settings);
            #endregion
        }
        internal void LogApplicationError(string message)
        {
            #region LogApplicationError
            m = new SmtpMail(GONDEREN_EMAIL_ADRESS, GONDEREN_ADI, GONDERILECEK_EMAIL_ADRESS, GONDERILECEK_ADI, "Application Error", METIN_FORMATI, message);
            settings = new SmtpServerSettings(SMTP_SERVER, USER_NAME, PASSWORD, true, Int32.Parse(SMTP_PORT));
            m.MailGonder(settings);
            #endregion

        }
        public void LogException(Exception ex)
        {
            try
            {
                #region LogException
                SqlException sqlEx = ex as SqlException;
                if (sqlEx != null)
                {
                    LogSqlErrors("+++++++ SQL SERVER EXCEPTION +++++++" + DateTime.Now.ToString() + Environment.NewLine + ex.Message + ex.StackTrace, sqlEx.Errors);
                }
                else
                {
                    LogApplicationError("+++++++ APPLICATION EXCEPTION +++++++" + DateTime.Now.ToString() + "\r\n" + "Message : " + ex.Message + Environment.NewLine + "Source : " + ex.Source + Environment.NewLine + "StackTrace : " + ex.StackTrace + Environment.NewLine + "Inner Exception : " + ex.InnerException + Environment.NewLine + Environment.NewLine);
                }
                #endregion
            }
            catch (Exception UnexpecEx)
            {
                ExceptionManaging exmanage = new ExceptionManaging();
                exmanage.MailLogSmtpServerError(UnexpecEx);
            }
        }

    }
}
