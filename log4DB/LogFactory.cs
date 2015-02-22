using System;

namespace log4DB
{
    public class LogFactory
    {
        private ILogger _logger;
        private LogTypes _logType;
        public LogFactory(LogTypes logTipleri)
        {
            switch (logTipleri)
            {
                case LogTypes.MailLogger:
                    _logType = logTipleri;
                    _logger = new MailLogger();
                    break;
                case LogTypes.DatabaseLogger:
                    _logType = logTipleri;
                    _logger = new DatabaseLogger();
                    break;
                case LogTypes.All:
                    _logType = logTipleri;
                    break;
            }
        }
        public void LogException(Exception ex)
        {
            if (_logType != LogTypes.All)
                _logger.LogException(ex);
            else
            {
                AllLogs(ex);
            }
        }

        private void AllLogs(Exception ex)
        {

            ILogger[] logCollection = { 
                                        new MailLogger(),
                                        new DatabaseLogger(),                   
                                      };
            foreach (ILogger logItem in logCollection)
            {
                logItem.LogException(ex);
            }
        }

    }
}
