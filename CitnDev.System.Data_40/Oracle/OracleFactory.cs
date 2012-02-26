using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CitnDev.System.Data.Oracle
{
    /// <summary>
    /// Provider factory for Oracle
    /// </summary>
    public class OracleFactory : DbProviderFactory
    {
        protected static Type DbConnectionType;
        protected static Type DbCommandType;
        protected static Type DbParameterType;
        protected static Type DbDataAdapterType;

        protected static bool UseDbProviderFactory;
        protected static DbProviderFactory DbProvider;

        static OracleFactory()
        {
            var oracleProviders = DbProviderFactories.GetFactoryClasses().AsEnumerable().Where(row => (string)row["InvariantName"] == "Oracle.DataAccess.Client").ToList();

            if (oracleProviders.Count == 0)
            {
                UseDbProviderFactory = false;
                var ofiBestOracleAssembly = AssemblyLoader.GetBestAssembly("Oracle.DataAccess");

                if (ofiBestOracleAssembly != null) { Debug.WriteLine("Best version found : " + ofiBestOracleAssembly); }
                else { Debug.WriteLine("No version found."); }

                if (ofiBestOracleAssembly != null)
                {
                    var assembly = Assembly.LoadFile(ofiBestOracleAssembly.FullName);
                    Debug.WriteLine("Assembly oracle : " + ofiBestOracleAssembly.FullName);
                    if (assembly != null)
                    {
                        DbConnectionType = assembly.GetType("Oracle.DataAccess.Client.OracleConnection");
                        DbCommandType = assembly.GetType("Oracle.DataAccess.Client.OracleCommand");
                        DbParameterType = assembly.GetType("Oracle.DataAccess.Client.OracleParameter");
                        DbDataAdapterType = assembly.GetType("Oracle.DataAccess.Client.OracleDataAdapter");
                        //DbTypeType = assembly.GetType("Oracle.DataAccess.Client.OracleDbType");
                    }
                }
            }
            else if (oracleProviders.Count == 1)
            {
                UseDbProviderFactory = true;
                var providerName = (string)oracleProviders[0]["InvariantName"];
                DbProvider = DbProviderFactories.GetFactory(providerName);
            }
            else
            {
                throw new Exception("Several oracle client factories.");
            }
        }

        internal static IDbCommand CreateInternalCommand()
        {
            if (UseDbProviderFactory)
                return DbProvider.CreateCommand();

            return (IDbCommand)Activator.CreateInstance(DbCommandType);
        }

        public override DbCommand CreateCommand()
        {
            return new DbOracleCommandTrace();
        }

        public override DbConnection CreateConnection()
        {
            if (UseDbProviderFactory)
                return DbProvider.CreateConnection();

            return (DbConnection)Activator.CreateInstance(DbConnectionType);
        }

        public override DbParameter CreateParameter()
        {
            if (UseDbProviderFactory)
                return DbProvider.CreateParameter();

            return (DbParameter)Activator.CreateInstance(DbParameterType);
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            if (UseDbProviderFactory)
                return DbProvider.CreateDataAdapter();

            return (DbDataAdapter)Activator.CreateInstance(DbDataAdapterType);
        }
    }
}
