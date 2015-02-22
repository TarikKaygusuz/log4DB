using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4DB;

namespace log4DB.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int y = 0;
                double i = 10 / y;
            }
            catch (Exception ex)
            {
                LogFactory log = new LogFactory(LogTypes.MailLogger);
                log.LogException(ex);
               
            }
        }
    }
}
