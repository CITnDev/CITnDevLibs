using System;

namespace CitnDev.System.Data.Oracle
{
    /// <summary>
    /// Tool class to help Oracle SQL usage
    /// </summary>
    public static class OracleHelper
    {
        #region Text

        /// <summary>
        /// If value is null or empty, return NULL or the value converted for Oracle SQL query
        /// </summary>
        /// <param name="value">Text</param>
        /// <returns>Text converted for oracle query</returns>
        public static string SQLConvertString(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("'", "''");
                value = value.Replace("&", "' || '&' || '");

                return "'" + value + "'";
            }

            return string.IsNullOrWhiteSpace(value) ? "NULL" : value;
        }

        #endregion

        #region Date & Time

        /// <summary>
        /// Convert DateTime to TO_DATE('value','YYYYMMDD')
        /// </summary>
        /// <param name="value">Date</param>
        /// <returns>Date converted for oracle query</returns>
        public static string SQLConvertDate(DateTime value)
        {
            return string.Format("TO_DATE('{0}','YYYYMMDD')", value.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// Convert DateTime to TO_DATE('value','YYYYMMDDHH24MISS')
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>DateTime converted for oracle query</returns>
        public static string SQLConvertDateTime(DateTime value)
        {
            return string.Format("TO_DATE('{0}','YYYYMMDDHH24MISS')", value.ToString("yyyyMMddHHmmss"));
        }

        /// <summary>
        /// Convert DateTime to TO_TIMESTAMP('value','YYYYMMDDHH24MISSFF3')
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>DateTime converted for oracle query</returns>
        public static string SQLConvertTimeStamp(DateTime value)
        {
            return string.Format("TO_TIMESTAMP('{0}','YYYYMMDDHH24MISSFF3')", value.ToString("yyyyMMddHHmmssfff"));
        }

        /// <summary>
        /// Convert DateTime to TO_CHAR(TO_DATE('value','YYYYMMDD')))
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>DateTime converted for oracle query</returns>
        public static string SQLConvertDateToChar(DateTime value)
        {
            return string.Format("TO_CHAR(TO_DATE('{0}','YYYYMMDD'))", value.ToString("yyyyMMdd"));
        }

        #endregion

        #region Connection string

        /// <summary>
        /// Generate the minimum connection string. The connection string looks like
        /// Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = server)(PORT = port)))(CONNECT_DATA = (SID = sid)));User Id=username;Password=password;
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="server">Server name</param>
        /// <param name="sid">Database SID</param>
        /// <param name="port">Port of the server. Default value is 1521</param>
        /// <returns>Default connection string</returns>
        public static string GetFullConnectionString(string userName, string password, string server, string sid, int port=1521)
        {
            return string.Format("Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1})))(CONNECT_DATA = (SID = {2})));User Id={3};Password={4};", server, port, sid, userName, password);
        }

        /// <summary>
        /// Generate the minimum connection string. The connection string looks like
        /// Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = server)(PORT = port)))(CONNECT_DATA = (SID = sid)));User Id=username;Password=password;
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="server">Server name</param>
        /// <param name="sid">Database SID</param>
        /// <param name="port">Port of the server. Default value is 1521</param>
        /// <returns>Default connection string</returns>
        public static string GetFullConnectionString(string userName, string password, string server, string sid, TimeSpan timeOut, int port = 1521)
        {
            return string.Format("Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1})))(CONNECT_DATA = (SID = {2})));User Id={3};Password={4};Connection Timeout={5}", server, port, sid, userName, password, (int)timeOut.TotalSeconds);
        }

        /// <summary>
        /// Generate the minimum connection string. The connection string looks like
        /// Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = server)(PORT = port)))(CONNECT_DATA = (SID = sid)));User Id=username;Password=password;
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="server">Server name</param>
        /// <param name="sid">Database SID</param>
        /// <param name="port">Port of the server. Default value is 1521</param>
        /// <returns>Default connection string</returns>
        public static string GetFullConnectionString(string userName, string password, string server, string sid, int timeOutInSecond, int port = 1521)
        {
            return string.Format("Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1})))(CONNECT_DATA = (SID = {2})));User Id={3};Password={4};Connection Timeout={5}", server, port, sid, userName, password, timeOutInSecond);
        }

        #endregion
    }
}
