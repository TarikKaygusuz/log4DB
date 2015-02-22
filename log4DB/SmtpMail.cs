using System.Net;
using System.Net.Mail;

namespace log4DB
{
    internal class SmtpMail
    {
        private string _gonderenEmailAdres;
        internal string GonderenEmailAdres
        {
            get { return _gonderenEmailAdres; }
            set { _gonderenEmailAdres = value; }

        }

        private string _gonderenDisplayName;
        internal string GonderenDisplayName
        {
            get { return _gonderenDisplayName; }
            set { _gonderenDisplayName = value; }

        }

        private string _gonderilecekEMailAdres;
        internal string GonderilecekEMailAdres
        {
            get { return _gonderilecekEMailAdres; }
            set { _gonderilecekEMailAdres = value; }
        }

        private string _gonderilecekDisplayName;
        internal string GonderilecekDisplayName
        {
            get { return _gonderilecekDisplayName; }
            set { _gonderilecekDisplayName = value; }

        }

        private string _subject;
        internal string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        private SmtpFormat _metinFormati;
        internal SmtpFormat MetinFormati
        {
            get { return _metinFormati; }
            set { _metinFormati = value; }
        }

        private string _metin;
        internal string Metin
        {
            get { return _metin; }
            set { _metin = value; }
        }

        internal SmtpMail()
        {
        }
        public SmtpMail(string gonderenEmailAdres, string gonderenDisplayName, string gonderilecekEmailAdres, string gonderilecekDisplayName, string subject, SmtpFormat metinFormati, string metin)
        {
            this._gonderenEmailAdres = gonderenEmailAdres;
            this._gonderenDisplayName = gonderenDisplayName;
            this._gonderilecekEMailAdres = gonderilecekEmailAdres;
            this._gonderilecekDisplayName = gonderilecekDisplayName;
            this._subject = subject;
            this._metinFormati = metinFormati;
            this._metin = metin;
        }

        public virtual void MailGonder(SmtpServerSettings smtpServerSttgs)
        {
            MailMessage mm = new MailMessage();
            MailAddress fromAdress = new MailAddress(GonderenEmailAdres, GonderenDisplayName);
            mm.From = fromAdress;
            mm.To.Add(new MailAddress(GonderilecekEMailAdres, GonderilecekDisplayName));

            mm.Subject = Subject;
            mm.Body = Metin;

            switch (MetinFormati)
            {
                case SmtpFormat.Text:
                    mm.IsBodyHtml = false;
                    break;
                case SmtpFormat.Html:
                    mm.IsBodyHtml = true;
                    break;
            }

            SmtpClient sc = new SmtpClient(smtpServerSttgs.SmtpserverIP, smtpServerSttgs.Port);
            sc.EnableSsl = smtpServerSttgs.EnableSll;
            sc.Credentials = new NetworkCredential(smtpServerSttgs.UserName, smtpServerSttgs.PassWord);
            sc.Send(mm);
        }
    }
    internal class SmtpServerSettings
    {
        private string _smtpserverIP;
        public string SmtpserverIP
        {
            get { return _smtpserverIP; }
            set { _smtpserverIP = value; }
        }

        private int _port;
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _passWord;
        public string PassWord
        {
            get { return _passWord; }
            set { _passWord = value; }
        }

        private bool _enableSll;
        public bool EnableSll
        {
            get { return _enableSll; }
            set { _enableSll = value; }
        }

        public SmtpServerSettings(string smtpServerIP, string userName, string passWord, bool enableSsl, int port)
        {
            this._smtpserverIP = smtpServerIP;
            this._userName = userName;
            this._passWord = passWord;
            this._enableSll = enableSsl;
            this._port = port;
        }
    }
    internal enum SmtpFormat
    {
        Text,
        Html
    }
}
