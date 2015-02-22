using System;

namespace log4DB
{

    internal interface ILogger
    {
        /// <summary>
        ///Gelen exception'in türüne göre loglama yapar. (SqlExceptions veya genel Application Exceptions)
        /// </summary>
        void LogException(Exception ex);

    }

}
