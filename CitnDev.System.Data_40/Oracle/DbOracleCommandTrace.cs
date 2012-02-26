using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace CitnDev.System.Data.Oracle
{
    public class DbOracleCommandTrace : DbCommand, ICommandToSql
    {
        private DbCommand _oOracleCommand = (DbCommand)OracleFactory.CreateInternalCommand();

        internal DbCommand OracleCommand
        {
            get { return _oOracleCommand; }
            set { _oOracleCommand = value; }
        }

        public override void Cancel()
        {
            _oOracleCommand.Cancel();
        }

        public override string CommandText
        {
            get
            {
                return _oOracleCommand.CommandText;
            }
            set
            {
                _oOracleCommand.CommandText = value;
            }
        }

        public override int CommandTimeout
        {
            get
            {
                return _oOracleCommand.CommandTimeout;
            }
            set
            {
                _oOracleCommand.CommandTimeout = value;
            }
        }

        public override CommandType CommandType
        {
            get
            {
                return _oOracleCommand.CommandType;
            }
            set
            {
                _oOracleCommand.CommandType = value;
            }
        }

        protected override DbParameter CreateDbParameter()
        {
            return _oOracleCommand.CreateParameter();
        }

        protected override DbConnection DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return _oOracleCommand.Parameters; }
        }

        protected override DbTransaction DbTransaction { get; set; }

        public override bool DesignTimeVisible
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            _oOracleCommand.Connection = DbConnection;
            _oOracleCommand.Transaction = DbTransaction;
            var drValue = _oOracleCommand.ExecuteReader(behavior);
            _oOracleCommand.Connection = null;
            _oOracleCommand.Transaction = null;

            return drValue; 
        }

        public override int ExecuteNonQuery()
        {
            _oOracleCommand.Connection = DbConnection;
            _oOracleCommand.Transaction = DbTransaction;
            var intValue = _oOracleCommand.ExecuteNonQuery();
            _oOracleCommand.Connection = null;
            _oOracleCommand.Transaction = null;

            return intValue;
        }

        public override object ExecuteScalar()
        {
            _oOracleCommand.Connection = DbConnection;
            _oOracleCommand.Transaction = DbTransaction;
            var oValue = _oOracleCommand.ExecuteScalar();
            _oOracleCommand.Connection = null;
            _oOracleCommand.Transaction = null;

            return oValue;
        }

        public override void Prepare()
        {
            _oOracleCommand.Prepare();
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get
            {
                return _oOracleCommand.UpdatedRowSource;
            }
            set
            {
                _oOracleCommand.UpdatedRowSource = value;
            }
        }

        private static string Interpret(DbCommand poCommand)
        {
            var sbQuery = new StringBuilder();

            if (poCommand.Parameters.Count == 0)
            {
                sbQuery.Append(poCommand.CommandText);
            }
            else
            {
                var oRegex = new Regex(@"(?<Parameters>:[\da-zA-Z0-9_]+)([ ]*([,\)]|AND|OR)|$)");
                var oMatchCollection = oRegex.Matches(poCommand.CommandText);

                if (oMatchCollection.Count != poCommand.Parameters.Count)
                {
// ReSharper disable InvocationIsSkipped
                    Debug.WriteLine("Number of parameters in query is not equals to number of parameters set in the command object " + poCommand.CommandText);
// ReSharper restore InvocationIsSkipped
                }
                else
                {
                    var strQuery = poCommand.CommandText;

                    for (var i = 0; i < oMatchCollection.Count; i++)
                    {
                        var strParameter = oMatchCollection[i].Groups["Parameters"].Captures[0].Value;
                        if (!string.IsNullOrEmpty(strParameter))
                        {
                            if (poCommand.Parameters[i].Value is DateTime)
                            {
                                var dt = (DateTime)poCommand.Parameters[i].Value;
                                strQuery = strQuery.Replace(strParameter, dt.Date == dt ? OracleHelper.SQLConvertDate(dt) : OracleHelper.SQLConvertDateTime(dt));
                            }
                            else if (poCommand.Parameters[i].Value is string)
                                strQuery = strQuery.Replace(strParameter, OracleHelper.SQLConvertString(poCommand.Parameters[i].ToString()));
                            else
                                strQuery = strQuery.Replace(strParameter, poCommand.Parameters[i].ToString());
                        }
                    }
                }
            }

            return sbQuery.ToString();
        }

        public string Interpret(IDbCommand poCommand)
        {
            if (poCommand.CommandText.EndsWith("\r\n"))
                poCommand.CommandText = poCommand.CommandText.Substring(0, poCommand.CommandText.Length - 2) + " \r\n";
            else
                poCommand.CommandText += " ";

            if (poCommand.Parameters.Count == 0)
                return poCommand.CommandText;

            var oRegex = new Regex(@"(?<Parameters>:[\da-zA-Z0-9_]+ )([\s]*|([,\)]|AND|OR)|$)");
            MatchCollection oMatchCollection = oRegex.Matches(poCommand.CommandText);

            if (oMatchCollection.Count != poCommand.Parameters.Count)
            {
                // ReSharper disable InvocationIsSkipped
                Debug.WriteLine(
                    "Number of parameters in query is not equals to number of parameters set in the command object " +
                    poCommand.CommandText);
                // ReSharper restore InvocationIsSkipped
                var msg =
                    "Number of parameters in query is not equals to number of parameters set in the command object : " + poCommand.CommandText + "\r\n" +
                    "Query params :\r\n";

                foreach (Match match in oMatchCollection)
                {
                    msg += "\t" + match.Value + "\r\n";
                }

                msg += "\nCommand params :\r\n";

                foreach (IDataParameter param in poCommand.Parameters)
                {
                    msg += "\t" + param.ParameterName + " = " + Convert.ToString(param) + "\r\n";
                }

                throw new Exception(msg);
            }

            string strQuery = poCommand.CommandText;

            for (int i = 0; i < oMatchCollection.Count; i++)
            {
                string strParameter = oMatchCollection[i].Groups["Parameters"].Captures[0].Value;
                if (!string.IsNullOrEmpty(strParameter))
                {
                    var param = (IDbDataParameter)poCommand.Parameters[i];
                    if (param.Value is DateTime)
                    {
                        var dt = (DateTime)param.Value;
                        strQuery = strQuery.Replace(strParameter,
                                                    dt.Date == dt
                                                        ? OracleHelper.SQLConvertDate(dt) + " "
                                                        : OracleHelper.SQLConvertDateTime(dt) + " ");
                    }
                    else if (param.Value is string)
                        strQuery = strQuery.Replace(strParameter, OracleHelper.SQLConvertString(param.Value.ToString()) + " ");
                    else if (param.Value is Int16 || param.Value is Int32 || param.Value is Int64)
                        strQuery = strQuery.Replace(strParameter, param.Value + " ");
                    else if (param.Value is decimal || param.Value is float || param.Value is double)
                        strQuery = strQuery.Replace(strParameter, param.Value.ToString().Replace(',', '.').Replace(" ", "") + " ");
                    else
                        throw new NotImplementedException(param.Value.GetType() + " is not implemented yet.");
                }
            }
            return strQuery;
        }

    }
}
