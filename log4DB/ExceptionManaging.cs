using System;
using System.Collections.Generic;
using System.Text;

namespace log4DB
{
    internal sealed class ExceptionManaging
    {
        IO i = null;
        MailLogger m = new MailLogger();

        internal void MailLogSmtpServerError(Exception ex)
        {
            try
            {
                i = new IO();
                i.AppendtoText("SmtpMailErrors.txt", "+++++++ log4DB +++++++" + DateTime.Now.ToString() + "\r\n" + "Message : " + ex.Message + Environment.NewLine + "Source : " + ex.Source + Environment.NewLine + "StackTrace : " + ex.StackTrace + Environment.NewLine + "Inner Exception : " + ex.InnerException + Environment.NewLine + Environment.NewLine + "Log verisi mail olarak gönderilemedi." + Environment.NewLine + Environment.NewLine);
            }
            catch
            { }
        }
        internal void DatabaseLogUnexpedtedError(Exception ex)
        {
            try
            {
                m.LogApplicationError("+++++++ log4DB +++++++" + DateTime.Now.ToString() + "\r\n" + "Message : " + ex.Message + Environment.NewLine + "Source : " + ex.Source + Environment.NewLine + "StackTrace : " + ex.StackTrace + Environment.NewLine + "Inner Exception : " + ex.InnerException + Environment.NewLine + Environment.NewLine + "Log verisi veritabanına kayıt edilemedi. ");
            }
            catch
            { }
        }
    }
}
